// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32947,y:32624,varname:node_4795,prsc:2|emission-2393-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32102,y:32460,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:251c6d1715f75714dab35327fe850be4,ntxv:2,isnm:False|UVIN-5567-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32682,y:32697,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB,D-9248-OUT,E-1365-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32286,y:32743,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32286,y:32895,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:9248,x:32265,y:33092,varname:node_9248,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:1365,x:32520,y:32895,varname:node_1365,prsc:2|A-6074-A,B-797-A;n:type:ShaderForge.SFN_Time,id:8035,x:31304,y:32369,varname:node_8035,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:5405,x:31207,y:32615,ptovrint:False,ptlb:x_speed,ptin:_x_speed,varname:node_5405,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:4750,x:31207,y:32708,ptovrint:False,ptlb:y_speed,ptin:_y_speed,varname:_node_5405_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Append,id:2327,x:31416,y:32615,varname:node_2327,prsc:2|A-5405-OUT,B-4750-OUT;n:type:ShaderForge.SFN_Multiply,id:1037,x:31615,y:32504,varname:node_1037,prsc:2|A-8035-T,B-2327-OUT;n:type:ShaderForge.SFN_TexCoord,id:2377,x:31594,y:32325,varname:node_2377,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:5567,x:31897,y:32569,varname:node_5567,prsc:2|A-2377-UVOUT,B-1037-OUT;proporder:6074-797-5405-4750;pass:END;sub:END;*/

Shader "Shader Forge/Laser_Shader" {
    Properties {
        _MainTex ("MainTex", 2D) = "black" {}
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _x_speed ("x_speed", Float ) = 0
        _y_speed ("y_speed", Float ) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform float _x_speed;
            uniform float _y_speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_8035 = _Time;
                float2 node_5567 = (i.uv0+(node_8035.g*float2(_x_speed,_y_speed)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_5567, _MainTex));
                float3 emissive = (_MainTex_var.rgb*i.vertexColor.rgb*_TintColor.rgb*2.0*(_MainTex_var.a*_TintColor.a));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
