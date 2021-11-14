Shader "Custom/Actor" {
    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Smoothness ("Smoothness", Range(0.0, 1.0)) = 0.5
        _Metallic ("Metallic", Range(0.0, 1.0)) = 0.0
        _OpenPct ("Open Pct", Range(0.0, 1.0)) = 0.0
        _OpenRect ("Open Rect", Vector) = (0.0, 0.0, 0.0, 0.0)
    }

    SubShader {
        Tags {
            "RenderType"="Opaque"
        }

        LOD 200

        CGPROGRAM
        // -- config --
        #pragma surface DrawSurf Standard fullforwardshadows
        #pragma target 3.0

        // -- types --
        struct Input {
            float2 uv_MainTex;
        };

        // -- props --
        /// albedo texture
        sampler2D _MainTex;

        /// albedo color
        fixed4 _Color;

        /// glossiness
        half _Smoothness;

        /// metallic
        half _Metallic;

        /// the max offset
        float _OffsetMax;

        /// the current percent offset
        float _OpenPct;

        /// the bottom-left & top-right corners of the offset rect
        float4 _OpenRect;

        // -- cool --
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        // -- program --
        void DrawSurf (Input i, inout SurfaceOutputStandard o) {
            float2 uv = i.uv_MainTex;

            // check if this uv is offset
            float y0 = _OpenRect.y;
            float y1 = _OpenRect.w;

            bool isOpen = (
                uv.x > _OpenRect.x &&
                uv.x < _OpenRect.z &&
                uv.y > lerp(y1, y0, saturate(_OpenPct)) &&
                uv.y < y1
            );

            // sample the texture
            fixed4 c = fixed4(0.0f, 0.0f, 0.0f, 0.0f);
            if (!isOpen) {
                c = tex2D(_MainTex, uv) * _Color;
            }

            // update output
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
