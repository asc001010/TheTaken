// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Transform Shader Built-in"
{
	Properties
	{
		_BaseColor_1("Base Color_1", 2D) = "white" {}
		_ColorSkin_Human("ColorSkin_Human", Color) = (1,1,1,0)
		_DirtMask("DirtMask", 2D) = "white" {}
		_Dirt("Dirt", Range( 0 , 2)) = 0
		_BloodMask("BloodMask", 2D) = "white" {}
		_Blood("Blood", Range( 0 , 1)) = 0
		_DirtColor("DirtColor", Color) = (0.2235294,0.1372549,0.04705883,1)
		_BloodColor("Blood Color", Color) = (0.227451,0,0,1)
		_Normal_1("Normal_1", 2D) = "white" {}
		_AO("AO", 2D) = "white" {}
		_Roughness("Roughness", 2D) = "white" {}
		_BloodRoughness("Blood  Roughness", Range( 0 , 1)) = 0
		_Human_Roughness("Human_Roughness", Range( 0 , 2)) = 1
		_DirtRoughness("Dirt  Roughness", Range( 0 , 0.5)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal_1;
		uniform float4 _Normal_1_ST;
		uniform sampler2D _BaseColor_1;
		uniform float4 _BaseColor_1_ST;
		uniform float4 _ColorSkin_Human;
		uniform float4 _DirtColor;
		uniform float _Dirt;
		uniform sampler2D _DirtMask;
		uniform float4 _DirtMask_ST;
		uniform float4 _BloodColor;
		uniform float _Blood;
		uniform sampler2D _BloodMask;
		uniform float4 _BloodMask_ST;
		uniform sampler2D _Roughness;
		uniform float4 _Roughness_ST;
		uniform float _Human_Roughness;
		uniform float _DirtRoughness;
		uniform float _BloodRoughness;
		uniform sampler2D _AO;
		uniform float4 _AO_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal_1 = i.uv_texcoord * _Normal_1_ST.xy + _Normal_1_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal_1, uv_Normal_1 ) );
			float2 uv_BaseColor_1 = i.uv_texcoord * _BaseColor_1_ST.xy + _BaseColor_1_ST.zw;
			float4 tex2DNode1 = tex2D( _BaseColor_1, uv_BaseColor_1 );
			float4 lerpResult5 = lerp( tex2DNode1 , ( _ColorSkin_Human * tex2DNode1 ) , 1.0);
			float2 uv_DirtMask = i.uv_texcoord * _DirtMask_ST.xy + _DirtMask_ST.zw;
			float lerpResult38 = lerp( 0.0 , _Dirt , tex2D( _DirtMask, uv_DirtMask ).r);
			float clampResult40 = clamp( lerpResult38 , 0.0 , 1.0 );
			float4 lerpResult47 = lerp( lerpResult5 , _DirtColor , clampResult40);
			float2 uv_BloodMask = i.uv_texcoord * _BloodMask_ST.xy + _BloodMask_ST.zw;
			float lerpResult43 = lerp( 0.0 , _Blood , tex2D( _BloodMask, uv_BloodMask ).r);
			float clampResult46 = clamp( lerpResult43 , 0.0 , 1.0 );
			float4 lerpResult48 = lerp( lerpResult47 , _BloodColor , clampResult46);
			o.Albedo = lerpResult48.rgb;
			float2 uv_Roughness = i.uv_texcoord * _Roughness_ST.xy + _Roughness_ST.zw;
			float lerpResult63 = lerp( ( tex2D( _Roughness, uv_Roughness ).r * _Human_Roughness ) , _DirtRoughness , lerpResult38);
			float lerpResult64 = lerp( lerpResult63 , _BloodRoughness , lerpResult43);
			o.Smoothness = lerpResult64;
			float2 uv_AO = i.uv_texcoord * _AO_ST.xy + _AO_ST.zw;
			o.Occlusion = tex2D( _AO, uv_AO ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.SamplerNode;32;-1283.97,703.5641;Inherit;True;Property;_TextureSample4;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;37;-1517.824,700.405;Inherit;True;Property;_DirtMask;DirtMask;2;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;38;-792.4919,665.7477;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;40;-570.0916,664.1476;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-933.3173,939.8274;Inherit;False;Constant;_Float5;Float 0;1;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;46;-536.8396,1054.342;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;47;377.8499,450.6312;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;49;85.17549,399.9317;Inherit;False;Property;_DirtColor;DirtColor;6;0;Create;True;0;0;0;False;0;False;0.2235294,0.1372549,0.04705883,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;41;-1250.718,1093.758;Inherit;True;Property;_TextureSample5;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;42;-1484.572,1090.599;Inherit;True;Property;_BloodMask;BloodMask;4;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;43;-759.2398,1055.942;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;72;-3511.943,1405.218;Inherit;True;Property;_Normal_1;Normal_1;8;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;71;-3222.984,1395.207;Inherit;True;Property;_TextureSample10;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;45;-1049.24,1045.742;Inherit;False;Property;_Blood;Blood;5;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1106.569,509.6331;Inherit;False;Constant;_Float4;Float 0;1;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-962.4917,559.5474;Inherit;False;Property;_Dirt;Dirt;3;0;Create;True;0;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;949.2028,757.1902;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Transform Shader Built-in;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SamplerNode;54;-3265.861,1700.577;Inherit;True;Property;_TextureSample7;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;55;-3501.994,1703.351;Inherit;True;Property;_Roughness;Roughness;10;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;59;-2854.961,1798.409;Inherit;False;Property;_Human_Roughness;Human_Roughness;12;0;Create;True;0;0;0;False;0;False;1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2556.728,1715.308;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;63;-2193.057,1712.442;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;64;-1865.457,1707.242;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-2424.404,1856.105;Inherit;False;Property;_DirtRoughness;Dirt  Roughness;13;0;Create;True;0;0;0;False;0;False;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-2059.119,1859.56;Inherit;False;Property;_BloodRoughness;Blood  Roughness;11;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;51;-687.371,1677.144;Inherit;True;Property;_TextureSample6;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;52;-921.225,1673.985;Inherit;True;Property;_AO;AO;9;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;48;421.6357,937.7018;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;50;163.5293,940.0059;Inherit;False;Property;_BloodColor;Blood Color;7;0;Create;True;0;0;0;False;0;False;0.227451,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1267.681,-476.2007;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-686.3484,-340.2977;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;5;-457.4062,-453.9017;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1501.535,-479.3597;Inherit;True;Property;_BaseColor_1;Base Color_1;0;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;6;-467.0795,-257.3317;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-910.0148,-666.2518;Inherit;False;Property;_ColorSkin_Human;ColorSkin_Human;1;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;32;0;37;0
WireConnection;38;0;35;0
WireConnection;38;1;39;0
WireConnection;38;2;32;1
WireConnection;40;0;38;0
WireConnection;46;0;43;0
WireConnection;47;0;5;0
WireConnection;47;1;49;0
WireConnection;47;2;40;0
WireConnection;41;0;42;0
WireConnection;43;0;44;0
WireConnection;43;1;45;0
WireConnection;43;2;41;1
WireConnection;71;0;72;0
WireConnection;0;0;48;0
WireConnection;0;1;71;0
WireConnection;0;4;64;0
WireConnection;0;5;51;0
WireConnection;54;0;55;0
WireConnection;56;0;54;1
WireConnection;56;1;59;0
WireConnection;63;0;56;0
WireConnection;63;1;65;0
WireConnection;63;2;38;0
WireConnection;64;0;63;0
WireConnection;64;1;66;0
WireConnection;64;2;43;0
WireConnection;51;0;52;0
WireConnection;48;0;47;0
WireConnection;48;1;50;0
WireConnection;48;2;46;0
WireConnection;1;0;2;0
WireConnection;3;0;4;0
WireConnection;3;1;1;0
WireConnection;5;0;1;0
WireConnection;5;1;3;0
WireConnection;5;2;6;0
ASEEND*/
//CHKSM=356861413F1066A2B0601F8BF0C63A5BFE41C705