
在Unity中我们通常可以用Profile和Profile Analyzer来对性能瓶颈进行分析统计，但是Unity的Profile只能记录到添加了BeginSample和EndSample的函数，如果想要看到某一帧完整的开销和堆栈调用，就需要手动的给所有函数头尾加上BeginSample和EndSample，不仅费时费力而且破坏了代码的整洁性。这里就提供了一种通过修改IL代码的方式来自动的对代码进行插桩。

首先我们知道在Unity中Asset/Scripts路径的脚本只是一种“资产”，在每次编译后最终都会集成在Library/ScriptAssemblies下的dll里，我们就可以通过一些工具对这些dll进行注入，以达到在每个函数首尾自动进行插桩的效果。

首先需要Mono.cecil这个库，这个库可以帮助我们读取和修改.Net程序集的元数据。然后还需要一个反编译的工具来帮我们验证注入是否成功。

```c#
using Mono.Cecil;  
using Mono.Cecil.Cil;  
using System;  
using System.IO;  
using System.Collections.Generic;  
using UnityEditor;  
using UnityEngine;  
using UnityEditor.Callbacks;  
using System.Linq;  
 public class HookEditor {  
    static List<string> assemblyPathss = new List<string>()  
    {  
        Application.dataPath+"/../Library/ScriptAssemblies/Assembly-CSharp.dll",  
        Application.dataPath+"/../Library/ScriptAssemblies/Assembly-CSharp-firstpass.dll",           
};  
     [MenuItem("Hook/主动注入代码")]  
    static void ReCompile()  
    {        AssemblyPostProcessorRun();  
    }     [MenuItem("Hook/输出结果")]  
    static void HookUtilsMessage()  
    {        HookUtils.ToMessage();  
    }     [PostProcessScene]//打包的时候会自动调用标记方法  
    static void AssemblyPostProcessorRun()  
    {        try  
        {  
            Debug.Log("AssemblyPostProcessor running");  
            EditorApplication.LockReloadAssemblies();  
            DefaultAssemblyResolver assemblyResolver = new DefaultAssemblyResolver();  
 int assembliesCount=AppDomain.CurrentDomain.GetAssemblies().Length;  
            Debug.Log("current  domain assemblies count is :"+assembliesCount);  
            int count = 1;  
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())  
            {                if(assembly.Location==string.Empty)  
                    continue;  
                string assemblyLocation = Path.GetDirectoryName(assembly.Location);  
                Debug.Log(count+++":   "+assemblyLocation);  
                assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(assembly.Location));  
            }             assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(EditorApplication.applicationPath) + "/Data/Managed");  
 ReaderParameters readerParameters = new ReaderParameters();  
            readerParameters.AssemblyResolver = assemblyResolver;  
 WriterParameters writerParameters = new WriterParameters();  
    
foreach (String assemblyPath in assemblyPathss)  
            {                readerParameters.ReadSymbols = true;  
                readerParameters.ReadWrite = true;  
                readerParameters.SymbolReaderProvider = new PortablePdbReaderProvider();  
                writerParameters.WriteSymbols = true;  
                writerParameters.SymbolWriterProvider = new PortablePdbWriterProvider();  
                 
AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath, readerParameters);  
 Debug.Log("Processing " + Path.GetFileName(assemblyPath));  
                if (HookEditor.ProcessAssembly(assemblyDefinition))  
                {                    Debug.Log("Writing to " + assemblyPath);  
                    assemblyDefinition.Write(writerParameters);  
                    Debug.Log("Done writing");  
                }                else  
                {  
                    Debug.Log(Path.GetFileName(assemblyPath) + " didn't need to be processed");  
                }            }        }        catch (Exception e)  
        {            Debug.LogWarning(e);  
        }        EditorApplication.UnlockReloadAssemblies();  
    } private static bool ProcessAssembly(AssemblyDefinition assemblyDefinition)  
    {        bool wasProcessed = false;  
 foreach (ModuleDefinition moduleDefinition in assemblyDefinition.Modules)  
        {            foreach (TypeDefinition typeDefinition in moduleDefinition.Types)  
            {                if (typeDefinition.Name == typeof(HookUtils).Name) continue;  
                //过滤抽象类  
                if (typeDefinition.IsAbstract) continue;  
                //过滤抽象方法  
                if (typeDefinition.IsInterface) continue;  
                foreach (MethodDefinition methodDefinition in typeDefinition.Methods)  
                {                    //过滤构造函数  
                    if(methodDefinition.Name == ".ctor")continue;  
                    if (methodDefinition.Name == ".cctor") continue;  
                    //过滤抽象方法、虚函数、get set 方法  
                    if (methodDefinition.IsAbstract) continue;  
                    if (methodDefinition.IsVirtual) continue;  
                    if (methodDefinition.IsGetter) continue;  
                    if (methodDefinition.IsSetter) continue;  
                    //如果注入代码失败，可以打开下面的输出看看卡在了那个方法上。  
                    //Debug.Log(methodDefinition.Name + "======= " + typeDefinition.Name + "======= " +typeDefinition.BaseType.GenericParameters +" ===== "+ moduleDefinition.Name);  
                    MethodReference logMethodReference = moduleDefinition.ImportReference(typeof(HookUtils).GetMethod("Begin", new Type[] { typeof(string) }));  
                    MethodReference logMethodReference1 = moduleDefinition.ImportReference(typeof(HookUtils).GetMethod("End", new Type[] { typeof(string) }));  
 ILProcessor ilProcessor = methodDefinition.Body.GetILProcessor();  
 Instruction first = methodDefinition.Body.Instructions[0];  
                    ilProcessor.InsertBefore(first, Instruction.Create(OpCodes.Ldstr, typeDefinition.FullName + "." + methodDefinition.Name));  
                    ilProcessor.InsertBefore(first, Instruction.Create(OpCodes.Call, logMethodReference));  
 //解决方法中直接 return 后无法统计的bug   
                    //https://lostechies.com/gabrielschenker/2009/11/26/writing-a-profiler-for-silverlight-applications-part-1/  
 Instruction last = methodDefinition.Body.Instructions[methodDefinition.Body.Instructions.Count - 1];  
                    Instruction lastInstruction = Instruction.Create(OpCodes.Ldstr, typeDefinition.FullName + "." + methodDefinition.Name);  
                    ilProcessor.InsertBefore(last, lastInstruction);  
                    ilProcessor.InsertBefore(last, Instruction.Create(OpCodes.Call, logMethodReference1));  
 var jumpInstructions = methodDefinition.Body.Instructions.Cast<Instruction>().Where(i => i.Operand == lastInstruction);  
                    foreach (var jump in jumpInstructions)  
                    {                        jump.Operand = lastInstruction;  
                    } wasProcessed = true;  
                }            }        } return wasProcessed;  
    }}
```

然后在编辑器点击注入代码,然后通过反编译软件打开dll即可看到在测试代码的首位加上了计时器

![[Pasted image 20240810095339.png]]

![Game-Dev-Knowledge-Summary/images/ILInject/Pasted image 20240810095339.png at main · shishouheng/Game-Dev-Knowledge-Summary (github.com)](https://github.com/shishouheng/Game-Dev-Knowledge-Summary/blob/main/images/ILInject/Pasted%20image%2020240810095339.png)