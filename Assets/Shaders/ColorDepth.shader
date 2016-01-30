// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:False,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,ufog:False,aust:False,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32764,y:32684|diff-17-OUT,spec-13-OUT,emission-40-OUT;n:type:ShaderForge.SFN_SceneDepth,id:2,x:33808,y:32640;n:type:ShaderForge.SFN_Vector1,id:13,x:33021,y:32733,v1:0;n:type:ShaderForge.SFN_Color,id:14,x:33477,y:32428,ptlb:Near_Color,ptin:_Near_Color,glob:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:16,x:33479,y:32589,ptlb:Far_Color,ptin:_Far_Color,glob:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Lerp,id:17,x:33264,y:32647|A-14-RGB,B-16-RGB,T-47-OUT;n:type:ShaderForge.SFN_ValueProperty,id:18,x:33819,y:32951,ptlb:Contrast,ptin:_Contrast,glob:False,v1:0.2;n:type:ShaderForge.SFN_ValueProperty,id:20,x:33975,y:32812,ptlb:Distance,ptin:_Distance,glob:False,v1:1;n:type:ShaderForge.SFN_ToggleProperty,id:39,x:33232,y:32912,ptlb:Self_Illum,ptin:_Self_Illum,on:True;n:type:ShaderForge.SFN_Multiply,id:40,x:33036,y:32813|A-17-OUT,B-39-OUT;n:type:ShaderForge.SFN_Divide,id:42,x:33813,y:32767|A-44-OUT,B-20-OUT;n:type:ShaderForge.SFN_Vector1,id:44,x:33971,y:32709,v1:1;n:type:ShaderForge.SFN_Multiply,id:46,x:33626,y:32756|A-2-OUT,B-42-OUT;n:type:ShaderForge.SFN_Power,id:47,x:33426,y:32853|VAL-46-OUT,EXP-48-OUT;n:type:ShaderForge.SFN_Clamp,id:48,x:33631,y:33020|IN-18-OUT,MIN-49-OUT,MAX-51-OUT;n:type:ShaderForge.SFN_Vector1,id:49,x:33819,y:33017,v1:0;n:type:ShaderForge.SFN_Vector1,id:51,x:33821,y:33082,v1:5;proporder:14-16-39-20-18;pass:END;sub:END;*/

Shader "ColorDepth" {
    Properties {
        _Near_Color ("Near_Color", Color) = (1,0,0,1)
        _Far_Color ("Far_Color", Color) = (0,0,0,1)
        [MaterialToggle] _Self_Illum ("Self_Illum", Float ) = 1
        _Distance ("Distance", Float ) = 1
        _Contrast ("Contrast", Float ) = 0.2
        _Offset ("Offset", Float ) = 10
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _Near_Color;
            uniform float4 _Far_Color;
            uniform float _Contrast;
            uniform float _Distance;
             uniform float _Offset;
            uniform fixed _Self_Illum;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float4 projPos : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);

                float node_2 = max(sceneZ-_Offset,0);
                float3 node_17 = lerp(_Near_Color.rgb,_Far_Color.rgb,pow((node_2*(1.0/_Distance)),clamp(_Contrast,0.0,5.0)));
                float3 emissive = (node_17*_Self_Illum);
           
                float3 finalColor = node_17+emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _Near_Color;
            uniform float4 _Far_Color;
            uniform float _Contrast;
            uniform float _Distance;
            uniform float _Offset;
            uniform fixed _Self_Illum;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float4 projPos : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                
                float node_2 = max(sceneZ-_Offset,0);
                float3 node_17 = lerp(_Near_Color.rgb,_Far_Color.rgb,pow((node_2*(1.0/_Distance)),clamp(_Contrast,0.0,5.0)));
                float3 finalColor = node_17;
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
