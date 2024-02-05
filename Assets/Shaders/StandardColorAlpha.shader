Shader "Custom/StandardColorAlpha" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
        
        _MetallicTex ("Metallic", 2D) = "white" {}
        _Metallic ("Metallic force", Range(0,1)) = 0.0
        
        _GlossinessTex ("Glossiness", 2D) = "white" {}
        _Glossiness ("Glossiness force", Range(0,1)) = 0.5
        
    } SubShader {
        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }
        LOD 200

        Cull back

        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MetallicTex;
        sampler2D _GlossinessTex;

        struct Input {
            float2 uv_MainTex;
        };

        
        fixed4 _Color;
        half _Glossiness;
        half _Metallic;
        
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 diffuse = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 metallic = tex2D (_MetallicTex, IN.uv_MainTex) * _Metallic;
            fixed4 glossiness = tex2D (_GlossinessTex, IN.uv_MainTex) * _Glossiness;
            
            o.Albedo = diffuse.rgb;
            o.Metallic = metallic;
            o.Smoothness = glossiness;
            o.Alpha = _Color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
