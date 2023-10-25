## 一、介绍
shader语言与一般编程语言的区别是它是由GPU执行的，而编程语言是由CPU执行的，并且是针对3D对象进行操作的代码

Shader的三种语言
- CG：是NVIDIA与微软共同开发的语言，用于在OpenGL和DirectX上运行的图形处理器编程语言
- HLSL：是基于DirectX的，主要在Windows平台上使用。HLSL和CG在语法和功能上非常相似
- GLSL：是基于OpenGL的，具有良好的跨平台性。

Shader的三种形式：
- FixedFunction Shader:固定功能着色器，只能实现基本的功能
- SurfaceShader:表面着色器，Unity自带的Shader模式，抽象层次较高
- VertexShader&FragameShader:顶点与片元着色器，最灵活，可以实现效果最多的一种着色器


## 二、基本着色器系列

## 1.NormalShaderFamily标准着色器系列
是unity内置的不透明shader，标准着色器是基于物理的渲染，可以让物体在不同的光照条件下得到不同的效果，标准着色器主要有以下几种
- Diffuse：漫反射，设置的是物体表面光照强度和颜色
- Normal Mapped：法线贴图，不改变对象的形状而是通过纹理来描述表面的细节，需要一个基础纹理和一个法线贴图
- Decal：贴花，在物体原本的外表上贴一张具有alpha通道的图片，需要一个基础纹理和一个具有alpha通道的贴纸图
- Vertex Lit：顶点着色
- Specular：高光属性，用于制作表面光滑油亮的效果，如金属、二次元人体的皮肤

## 2.TransparentShaderFamily透明着色器系列
透明着色器用于全透明或半透明对象，通过使用基础纹理的Alpha通道，可以确定对象的区域的透明度高于或者低于其他区域。

所有的计算原理和类型都与标准着色器差不多，运行在半透明或者完全透明的情况下进行渲染，主纹理需要RGBA四个通道，Alpha为0表示全透明，1表示完全不透明


# 三、表面着色器代码结构

底层提供的物体表面属性，可以通过对这些进行修改来实现不同的效果
```c#
struct SurfaceOutput 
{
 fixed3 Albedo;//基色 rgb
 fixed3 Normal;//法线向量 xyz
 fixed3 Emission;//自发光颜色
 half Specular;//高光指数
 fixed Gloss;//光泽度
 fixed Alpha;//透明度
};
```


![[shader code frame.png]]
shader代码的整体结构从上往下依次是
最上层是Shader 然后加上shader文件的路径和名字，如
```C#
//是一个位于Custom文件下的名为NewSurfaceShader的shader代码
Shader "Custom/NewSurfaceShader"
```

然后是Properties属性，属性定义了可以在Inspector窗口中编辑的变量，这些变量可以在Shader中使用，用以控制材质的行为和外观
```c#
Properties 
	{
		//格式：变量名（"显示名"，数据类型）=默认值

		//float:32位浮点数
		_Float("MyFloat",float)=1
		//range:范围浮点数
		_Range("亮度",range(0,100))=20
		//vector:浮点四元组,xyzw
		_Vector("MyVector",Vector)=(1,1,1,1)
		//color：浮点四元组,rgba
		_Color("Color",color)=(1,1,1,1)
		//2D:纹理贴图，2的N次方大小的贴图，sampler2D
		_2D("MyTexture",2D)="white"{}
		//rect:纹理贴图，非2的N次方大小的贴图 samplerRect
		_Rect("MyRect",rect)="white"{}
		//cube:立方体纹理
		_Cube("MyCube",cube)="white"{}
	}
```

在属性定义完后就可以写SubShader，一段shader程序可以有多个SubShader，但只会执行第一个可执行的SubShader，当编写的Shader需要在多种GPU下执行时，就可以编写多个SubShader来应对，如果所有的SubShader都无法执行，那么就会Fallback回滚执行默认的Shader

在每个SubShader中会有多个Pass，每个Pass会被依次执行，比如处理光照和阴影时，灯光写一个Pass，阴影写一个Pass，当GPU执行时，会先计算灯光Pass输出颜色，然后计算阴影Pass再输出颜色

```c#
Shader "Custom/NewSurfaceShader" {
	Properties 
	{
		//颜色 浮点四元组
		_Color("Color",color)=(1,1,1,1)
	}
	//每一个shader的子着色器列表
	//需要渲染时会自上而下提取第一个能运行在显卡上的着色器方案
	SubShader 
	{
		
	}

	SubShader
	{

	}

	SubShader
	{

	}
	//所有子着色器都不执行时，执行Diffus Shader
	FallBack "Diffuse"
}
```

# 四、表面着色器案例

## 1.漫反射

```c#
//定义一个在Custom路径下的名为NewSurfaceShader的Shader
Shader "Custom/NewSurfaceShader" 
{
	Properties 
	{
		//定义一个颜色属性，类型为color，初始值为（0，0，0，0）全透明
		_Color("Color",color)=(0,0,0,0)
		//定义一个纹理属性，类型为2D，初始值为white
		_MainTex("Base",2D)="white"{}
	}

	SubShader 
	{
		//开始编写CG程序
		CGPROGRAM
		//指定使用Lambert光照模型的表面着色器
		#pragma surface surf Lambert
		//声明_Color和_MainTex属性在CG程序中的变量
		fixed4 _Color;
		sampler2D _MainTex;
		
		//定义输入结构体
		struct Input
		{
			//_MainTex的uv坐标
			float2 uv_MainTex;
		};
		
		//定义表面着色器函数
		void surf(Input IN,inout SurfaceOutput o)
		{
			//根据_MainTex的UV坐标获取纹理颜色
			fixed col=tex2D(_MainTex,IN.uv_MainTex);
			//将物体的颜色设置为_color*纹理颜色
			o.Albedo=col*_Color;
		}
		//结束CG程序
		ENDCG
	}

	FallBack "Diffuse"
}
```

## 2.细节纹理
```c#
Shader "Custom/NewSurfaceShader" 
{
	Properties 
	{
		//颜色 浮点四元组
		_Color("Color",color)=(0,0,0,0)
		_MainTex("Base",2D)="white"{}
		_MainTex2("Base2",2D)="white"{}
		
	}

	SubShader 
	{
		CGPROGRAM
		//surface表示一段表面着色器
		//surf表面着色器的执行函数
		//Lambert 兰伯特光照模型
		#pragma surface surf Lambert
		//将在属性里定义的变量匹配到CG语言中
		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _MainTex2;

		struct Input
		{
			//_MainTex的uv坐标
			float2 uv_MainTex;
			float2 uv_MainTex2;
		};

		void surf(Input IN,inout SurfaceOutput o)
		{
			//获取贴图的uv坐标
			fixed col=tex2D(_MainTex,IN.uv_MainTex);
			fixed col2=tex2D(_MainTex2,IN.uv_MainTex2);
			o.Albedo=col*col2*_Color;
		}
		ENDCG
	}

	FallBack "Diffuse"
}

```

## 3.法线纹理
```c#
Shader "Custom/NewSurfaceShader" 
{
	Properties 
	{
		//颜色 浮点四元组
		_Color("Color",color)=(0,0,0,0)
		_MainTex("Base",2D)="white"{}
		_BumpMap("凹凸",2D)="bump"{}
	}

	SubShader 
	{
		CGPROGRAM
		//surface表示一段表面着色器
		//surf表面着色器的执行函数
		//Lambert 兰伯特光照模型
		#pragma surface surf Lambert
		//将在属性里定义的变量匹配到CG语言中
		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input
		{
			//_MainTex的uv坐标
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		void surf(Input IN,inout SurfaceOutput o)
		{
			//获取贴图的uv坐标
			fixed col=tex2D(_MainTex,IN.uv_MainTex);
			//将物体的颜色设置为_color*贴图
			o.Albedo=col*_Color;
			//tex2D获取的是颜色，然后通过UnPackNormal解码为法线向量
			o.Normal=UnpackNormal(tex2D(_BumpMap,IN.uv_BumpMap));
		}
		ENDCG
	}

	FallBack "Diffuse"
}
```

## 4.渐变边缘光照效果

实现渐变边缘发光效果需要通过点乘计算摄像机视角向量与物体的法线向量的值然后与物体的本身颜色相乘，最后将结果赋值给物体的自发光属性Emission。

当点乘结果为0，说明该位置法线向量与视角向量方向相同，说明这个位置是物体的中心区域，不需要进行发光

当点乘结果为1，说明该位置法线向量垂直于视角向量，说明这个位置是物体的上方或者下方，是发光最亮的区域

```c#
Shader "Custom/NewSurfaceShader" 
{
	Properties 
	{
		_MainColor("主颜色",color)=(1,1,1,1)
		_RimColor("自发光颜色",color)=(1,1,1,1)
	}

	SubShader 
	{
		CGPROGRAM
		#pragma surface surf Lambert
		fixed4 _RimColor;
		fixed4 _MainColor;

		struct Input
		{
			//视角向量
			float3 viewDir;
		};

		void surf(Input IN,inout SurfaceOutput o)
		{
			o.Albedo=_MainColor;
			//计算归一化视角向量与物体法线向量的点乘结果，并通过saturate将结果限制在0-1之间
			//用1减去上述结果，表明当观察方向与法线越接近时，结果越接近0，即不发光，反之则发光
			float rim=1-saturate(dot(normalize(IN.viewDir),o.Normal));
			o.Emission=_RimColor*rim;
		}
		ENDCG
	}

	FallBack "Diffuse"
}
```

## 5.通过顶点函数控制模型的大小
```c#
Shader "Custom/NewSurfaceShader" 
{
	Properties 
	{
		_MainColor("主颜色",color)=(1,1,1,1)
		_MainTex("主纹理",2D)="white"{}
		_Amount("Amount",range(0,1))=0.5
	}

	SubShader 
	{
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		sampler2D _MainTex;
		fixed _Amount;
		fixed4 _MainColor;

		struct Input
		{
			float2 uv_MainTex;
		};

		void vert(inout appdata_full v)
		{
			v.vertex.xyz+=v.normal*_Amount;
		}

		void surf(Input IN,inout SurfaceOutput o)
		{
			fixed4 col=tex2D(_MainTex,IN.uv_MainTex);
			o.Albedo=col.rgb*_MainColor;
		}
		ENDCG
	}

	FallBack "Diffuse"
}
```

## 6.通过点乘实现积雪效果
```c#
Shader "Custom/SnowShader" 
{
    Properties 
    {
       _MainTex("主纹理",2d)="white"{}
       _MainColor("基色",Color)=(1,1,1,1)
       _BumpMap("Bump",2d)="bump"{}

       //雪花的方向、颜色、厚度
       _SnowDirection("SnowDirection",Vector)=(0,1,0)
       _SnowColor("SnowColor",Color)=(1,1,1,1)
       _SnowLevel("SnowLevel",Range(0,1))=0
    }
 
    SubShader 
    {
        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _BumpMap;
        fixed4 _MainColor;
        float4 _SnowColor;
        float4 _SnowDirection;
        fixed _SnowLevel;
 
       
       struct Input 
       {
   		float2 uv_MainTex;
   		float2 uv_BumpMap;
   		//世界空间法向量
   		float3 worldNormal;INTERNAL_DATA
       };

 
       void surf (Input IN, inout SurfaceOutput o) 
       {
       	//获取_MainTex的颜色值
   		fixed4 c=tex2D(_MainTex,IN.uv_MainTex);
   		//将物体的法线向量设置为法线贴图的法线向量
   		o.Normal=UnpackNormal(tex2D(_BumpMap,IN.uv_BumpMap));

   		//通过点乘计算雪的方向向量与模型在世界空间的法线向量的结果来判断模型是否需要被雪覆盖
   		if(dot(WorldNormalVector(IN,o.Normal),normalize(_SnowDirection.xyz))>1-_SnowLevel)
   			o.Albedo=_SnowColor.rgb;
   		else
   			o.Albedo=c.rgb*_MainColor.rgb;

   		o.Alpha=c.a;
	   }

        ENDCG
    }
 
    FallBack "Diffuse"
}
```

## 7.配合简谐运动公式实现水面效果

实现水面波动的效果除了简谐运动公式还需要使用shader中的_Time变量，这是一个内置的全局变量，用于获取当前游戏的运行时间，可以搭配实现各种基于时间的效果   
Time是一个float4类型的变量，包含了四个分量(t/20,t,t * 2,t * 3)

```c#
Shader "Custom/WaterSurface" 
	{
	Properties 
	{
		_Color ("基色", Color) = (1,1,1,1)
		_MainTex ("主纹理 (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5

		_XSpeed("XSpeed",range(0,10))=0
		_YSpeed("YSpeed",range(0,10))=0

	}
	SubShader 
		{
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert

		//属性赋值
		sampler2D _MainTex;
		fixed4 _Color;
		float _XSpeed;
		float _YSpeed;

		//获取_MainTex的uv纹理坐标
		struct Input 
		{
			float2 uv_MainTex;
		};

		//appdata_base是顶点相关数据，在vert函数中修改顶点数据信息
		void vert(inout appdata_base v)
		{
			//简谐运动公式 A*sin(x+t)

			//旗帜波
			v.vertex.y+=1*sin(v.vertex.z+_Time.y);

			//涟漪
			//v.vertex.y+=0.2*sin(length(v.vertex.xz)+_Time.y*5);

			//水波
			//v.vertex.y+=2*sin((v.vertex.x+v.vertex.z)+_Time.y*2);
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			//uv随着时间进行偏移，即纹理滚动效果
			fixed2 scrolledUV=IN.uv_MainTex;
			fixed xscrollValue=_XSpeed*_Time.y;
			fixed yscrollValue=_YSpeed*_Time.y;

			scrolledUV+=fixed2(xscrollValue,yscrollValue);
			o.Albedo=tex2D(_MainTex,scrolledUV).rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
```

# 五、表面着色器执行顺序

![[surface shader sequence.png]]

表面着色器执行的过程中首先会收集模型的顶点信息，然后通过Vertex顶点的计算函数将顶点信息保存到Input结构体中，然后通过Surface函数来计算渲染效果，并把渲染的效果保存到SurfaceOutput结构体中，然后通过光照模型对上面的渲染效果进行光照计算，得到最终的渲染效果

- 1.收集模型的顶点信息，并根据顶点的位置、法线等属性计算得到所需的数据
- 2.通过顶点函数（Vertex函数）的计算，将顶点信息保存到Input结构体中，这样在后续的计算中就可以使用这些信息
- 3.通过Surface函数来计算渲染效果，将渲染效果保存到SurfaceOutput结构体中。Surface函数可以控制材质的颜色、透明度等属性，并且根据需要还可以对纹理进行采样
- 4.最后通过光照模型对SurfaceOutput的内容进行光照计算，得到最终的渲染效果。光照模型可以根据不同的光源和材质属性，计算出每个像素的颜色值

所以整个Surface Shader的执行过程就是收集顶点信息->顶点函数->Surface函数->光照计算