Shader "Custom/2DConeLight"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Light Color", Color) = (1, 1, 1, 1)
        _Position("Start Position", Vector) = (0, 0, 0, 0)
        _Angle("Cone Angle", Range(0, 180)) = 30.0
        _Intensity("Intensity", Float) = 1.0
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                // 쉐이더에서 사용할 모든 변수를 여기서 선언합니다.
                float4 _Position;
                float4 _Color;
                float _Angle;
                float _Intensity;

                sampler2D _MainTex;

                // 구조체 선언 - 변환 및 텍스처 좌표 처리용
                struct appdata
                {
                    float4 position : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 position : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                // 버텍스 쉐이더
                v2f vert(appdata v)
                {
                    v2f o;
                    o.position = UnityObjectToClipPos(v.position);
                    o.texcoord = v.texcoord;
                    return o;
                }

                // 프래그먼트 쉐이더
                half4 frag(v2f i) : SV_Target
                {
                    // 빛의 위치와 텍스처 좌표를 기반으로 원뿔 효과 계산
                    float2 lightPos = _Position.xy;
                    float2 toLight = i.texcoord - lightPos;

                    // 각도 계산
                    float distance = length(toLight);
                    float coneFactor = step(distance / tan(radians(_Angle)), 1.0);

                    // 텍스처 색상과 강도 적용
                    half4 texColor = tex2D(_MainTex, i.texcoord) * _Intensity;

                    // 원뿔 내에 있을 때만 색을 반환
                    return texColor * coneFactor;
                }

                ENDCG
            }
        }

 FallBack "Diffuse"
}