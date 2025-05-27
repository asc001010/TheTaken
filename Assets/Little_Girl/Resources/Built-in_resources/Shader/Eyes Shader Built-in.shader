// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Eyes Shader Built-in"
{
	Properties
	{
		_BaseColor("Base Color", 2D) = "white" {}
		_brightnessHuman("brightnessHuman", Range( 0 , 5)) = 1.5
		_IrisMask("IrisMask", 2D) = "white" {}
		_EmissiveMask("EmissiveMask", 2D) = "white" {}
		_ColorHumanEyes("ColorHumanEyes", Color) = (1,1,1,0)
		_Roughness("Roughness", Range( 0 , 1)) = 0.89
		_EmissionIntensity("EmissionIntensity", Range( 0 , 100)) = 0
		[HDR]_EmissionColor("EmissionColor", Color) = (1,0,0,0)
		[Toggle]_EmissiveMaskSwitch("EmissiveMask Switch", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _BaseColor;
		uniform float4 _BaseColor_ST;
		uniform float _brightnessHuman;
		uniform float4 _ColorHumanEyes;
		uniform sampler2D _IrisMask;
		uniform float4 _IrisMask_ST;
		uniform float _EmissiveMaskSwitch;
		uniform float4 _EmissionColor;
		uniform float _EmissionIntensity;
		uniform sampler2D _EmissiveMask;
		uniform float4 _EmissiveMask_ST;
		uniform float _Roughness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BaseColor = i.uv_texcoord * _BaseColor_ST.xy + _BaseColor_ST.zw;
			float4 temp_output_3_0 = ( tex2D( _BaseColor, uv_BaseColor ) * _brightnessHuman );
			float2 uv_IrisMask = i.uv_texcoord * _IrisMask_ST.xy + _IrisMask_ST.zw;
			float4 lerpResult8 = lerp( temp_output_3_0 , ( temp_output_3_0 * _ColorHumanEyes ) , tex2D( _IrisMask, uv_IrisMask ).r);
			o.Albedo = lerpResult8.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV56 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode56 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV56, -1.0 ) );
			float4 temp_output_55_0 = ( ( _EmissionColor * ( _EmissionIntensity * 100000.0 ) ) * fresnelNode56 );
			float2 uv_EmissiveMask = i.uv_texcoord * _EmissiveMask_ST.xy + _EmissiveMask_ST.zw;
			o.Emission = (( _EmissiveMaskSwitch )?( ( temp_output_55_0 * tex2D( _EmissiveMask, uv_EmissiveMask ).r ) ):( temp_output_55_0 )).rgb;
			o.Smoothness = _Roughness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;940.2587,224.339;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Eyes Shader Built-in;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.RangedFloatNode;51;607.8605,464.9123;Inherit;False;Property;_Roughness;Roughness;5;0;Create;True;0;0;0;False;0;False;0.89;0.918;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-949.275,-351.0795;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1210.187,-358.3947;Inherit;True;Property;_BaseColor;Base Color;0;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-465.0291,-273.5271;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-669.7304,-148.9382;Inherit;False;Property;_brightnessHuman;brightnessHuman;1;0;Create;True;0;0;0;False;0;False;1.5;1.34;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;-696.0972,45.37014;Inherit;False;Property;_ColorHumanEyes;ColorHumanEyes;4;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-461.6706,-23.7583;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;8;-88.49751,-159.7964;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;9;-787.5137,254.2249;Inherit;True;Property;_TextureSample1;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;10;-1048.426,246.9097;Inherit;True;Property;_IrisMask;IrisMask;2;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-793.8828,1079.736;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;56;-1073.75,1097.561;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-1245.385,1207.221;Inherit;False;Constant;_Float13;Float 10;11;0;Create;True;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-983.326,919.2841;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-774.2743,931.7812;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1275.24,903.5961;Inherit;False;Property;_EmissionIntensity;EmissionIntensity;6;0;Create;True;0;0;0;False;0;False;0;100;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-1163.734,1001.719;Inherit;False;Constant;_Float12;Float 10;11;0;Create;True;0;0;0;False;0;False;100000;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;58;-971.4778,727.8344;Inherit;False;Property;_EmissionColor;EmissionColor;7;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,0;1.267651E+30,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;65;-689.0084,1329.51;Inherit;True;Property;_TextureSample5;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;78bf728953ae5c140b4a58f8b36fc12d;78bf728953ae5c140b4a58f8b36fc12d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;64;-1033.821,1326.696;Inherit;True;Property;_EmissiveMask;EmissiveMask;3;0;Create;True;0;0;0;False;0;False;78bf728953ae5c140b4a58f8b36fc12d;78bf728953ae5c140b4a58f8b36fc12d;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-364.5274,1240.34;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;77;-213.9236,809.2594;Inherit;False;Property;_EmissiveMaskSwitch;EmissiveMask Switch;8;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
WireConnection;0;0;8;0
WireConnection;0;2;77;0
WireConnection;0;4;51;0
WireConnection;1;0;2;0
WireConnection;3;0;1;0
WireConnection;3;1;4;0
WireConnection;5;0;3;0
WireConnection;5;1;7;0
WireConnection;8;0;3;0
WireConnection;8;1;5;0
WireConnection;8;2;9;1
WireConnection;9;0;10;0
WireConnection;55;0;54;0
WireConnection;55;1;56;0
WireConnection;56;3;59;0
WireConnection;53;0;52;0
WireConnection;53;1;57;0
WireConnection;54;0;58;0
WireConnection;54;1;53;0
WireConnection;65;0;64;0
WireConnection;63;0;55;0
WireConnection;63;1;65;1
WireConnection;77;0;55;0
WireConnection;77;1;63;0
ASEEND*/
//CHKSM=8C57D60961380536043253724C066E7AF5D41781