
在Unity中可以通过创建standar surface shader来创建一个表面着色器，并且Unity会在其中定义好一些默认的参数和结构，这篇笔记就是对standar surface shader的代码结构进行介绍以对shader有一些初步认知。

如下就是一个默认创建的standar surface shader的结构

```
Shader "Custom/CustomDiffuse"  
{  
    Properties  
    {  
        _Color ("Color", Color) = (1,1,1,1)  
    }    
    SubShader  
    {  
        Tags { "RenderType"="Opaque" }  
        LOD 200  
  
        CGPROGRAM  
        // Physically based Standard lighting model, and enable shadows on all light types  
        #pragma surface surf Lambert fullforwardshadows  
  
        // Use shader model 3.0 target, to get nicer looking lighting  
        #pragma target 3.0  
  
        sampler2D _MainTex;  
  
        struct Input  
        {  
            float2 uv_MainTex;  
        };  
        fixed4 _Color;  
  
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.  
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.        // #pragma instancing_options assumeuniformscaling        UNITY_INSTANCING_BUFFER_START(Props)  
            // put more per-instance properties here  
        UNITY_INSTANCING_BUFFER_END(Props)  
  
        void surf (Input IN, inout SurfaceOutput o)  
        {            // Albedo comes from a texture tinted by color  
            fixed4 c = _Color;  
            o.Albedo = c.rgb;  
        }ENDCG  
    }  
    FallBack "Diffuse"  
}
```


### 1.Shader命名和Properties
```
Shader "Custom/CustomDiffuse"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }

```

- Shader "Custom/CustomDiffuse"定义了该shader的名称以及存放路径，便于在材质编辑器中选择和使用
- Properties定义了Shader在Inspector面板中可以调整的参数（如果要使参数有效还必须在后面代码块里声明同名变量）


### 2.SubShader和Tags

```
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
```

- SubShader:定义了Shader的一个渲染路径，Unity会根据硬件和其他条件选择合适的SubShader
- Tags { "RenderType"="Opaque" }：定义了渲染类型为不透明，这回影响渲染顺序和一些优化
- LOD 200：定义了这个歌SubShader的细节等级为200，用于选择合适的渲染效果


### 3. CGPROGRAM块

```
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

```

- **CGPROGRAM**: 开始CG/HLSL代码块，用于编写Shader的核心逻辑。
- **#pragma surface surf Standard fullforwardshadows**: 定义了一个表面着色器（surface shader），使用Unity的标准光照模型，并启用所有光源类型的阴影。
    - **surf**: 表面函数的名称（在后面定义）。
    - **Standard**: 使用Unity的标准物理光照模型。
    - **fullforwardshadows**: 启用前向渲染路径下的所有阴影效果。
- **#pragma target 3.0**: 指定了Shader目标的编译版本为Shader Model 3.0，提供更高级的图形功能。

### 4. 纹理采样器和输入结构

```
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

```

- **sampler2D _MainTex**: 定义了一个2D纹理采样器，。
- **struct Input**: 定义了传递给表面函数的输入结构。
    - **float2 uv_MainTex**: 定义了用于纹理坐标的输入变量。


### 5. 固定颜色和实例化支持

```
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

```

- **fixed4 _Color**: 定义了一个四维向量，用于存储颜色值(对应在Propertis声明的_Color)。
- **实例化支持代码块**: 提供了GPU实例化支持，可以在一个draw call中渲染多个实例以提高性能。
    - **UNITY_INSTANCING_BUFFER_START(Props)** 和 **UNITY_INSTANCING_BUFFER_END(Props)**: 定义了实例化缓冲区，可以在其中添加每个实例的特定属性


### 6.表面函数

```
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = _Color;
            o.Albedo = c.rgb;
        } 
        ENDCG
    }

```

**void surf (Input IN, inout SurfaceOutputStandard o)**: 定义了表面函数，负责计算每个像素的表面属性。

- **Input IN**: 输入结构，包含纹理坐标。
- **inout SurfaceOutputStandard o**: 输出结构，包含表面的物理属性。


### 7.回退shader

```
    FallBack "Diffuse"
}
```

**FallBack "Diffuse"**: 指定当当前Shader不支持时，回退到内置的“Diffuse” Shader。这确保了在不支持高级Shader的硬件上仍然可以正确渲染。