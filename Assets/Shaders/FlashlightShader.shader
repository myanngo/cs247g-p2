Shader "Custom/FlashlightShader"
{
    Properties
    {
        _MousePosition ("Mouse Position", Vector) = (0,0,0,0)
        _FlashlightSize ("Flashlight Size", Float) = 2.0
        _Darkness ("Darkness", Range(0,1)) = 0.85
    }
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent+1" 
            "RenderType"="Transparent" 
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
            };

            float4 _MousePosition;
            float _FlashlightSize;
            float _Darkness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 worldPos = i.worldPos.xy;
                float dist = distance(worldPos, _MousePosition.xy);
                
                // Create a soft-edged circle
                float circle = saturate(1 - dist / _FlashlightSize);
                circle = smoothstep(0, 1, circle);
                
                // Invert the circle to create darkness with a light hole
                float alpha = lerp(_Darkness, 0, circle);
                
                return fixed4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
} 