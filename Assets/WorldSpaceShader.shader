Shader "Custom/WorldSpaceTextureShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Scale("Scale", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Scale;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Compute world space UVs
                float3 worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv.x = worldPosition.x / _Scale;
                o.uv.y = worldPosition.z / _Scale; // Using X and Z for a horizontal surface

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the texture at the computed UVs
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
