Shader "Custom/line" {
	SubShader {
   		Tags { "Queue"="Transparent-1" "IgnoreProjector"="True" "RenderType"="Transparent" }
		ZWrite Off
		// Blend SrcAlpha One 				// alpha additive
		Blend SrcAlpha OneMinusSrcAlpha		// mix
		// Blend One OneMinusSrcAlpha		// premultiplied
		
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
 			#pragma target 3.0
 			
 			#include "UnityCG.cginc"

 			struct appdata_custom {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
			};

 			struct v2f {
 				float4 pos:SV_POSITION;
 				fixed4 color:COLOR;
 			};

            v2f vert(appdata_custom v)
            {
            	float4 tv = mul(UNITY_MATRIX_MVP, v.vertex);
            	v2f o;
            	o.pos = tv;
            	o.color = v.color;
            	return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
				return i.color;
            }

            ENDCG
        }
    }
}
