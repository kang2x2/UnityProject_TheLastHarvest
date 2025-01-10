Shader "Custom/ColorLerp"
{
    Properties
    {
        // 변수명, 익스펙터에서 보여줄 프로퍼티 이름, 프로퍼티, 초기값
        _MainTex("Texture", 2D) = "white" { }
        _Amount("FadeAmount", Range(0, 1)) = 0.0
    }

    Category
    {
        // "Queue" = "Overlay" : 해당 쉐이더가 화면에 렌더링될 순서, 투명 객체들보다 우선해서 렌더링.
        // "IgnoreProjector" = "True" : 그림자(프로젝터 효과)에 영향 받지 않음.
        // RenderType: "Transparent" : 투명 객체로 렌더링, 투명 효과가 있는 텍스쳐에 유용.
        Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        // Blend SrcAlpha One
        Blend SrcAlpha OneMinusSrcAlpha // 알파 블랜딩 사용 방식.
        Cull Off Lighting Off ZWrite Off Fog { Color(0, 0, 0, 0) }

        // Cull Off // 양면 렌더링. 백스페이스 컬링을 사용하지 않겠다.
        // Lighting Off // 조명 처리 없이 텍스쳐에 내장된 색상만 사용.
        // ZWrite Off // 깊이 버퍼 off.

        // 해당 데이터들을 쉐이더에서 사용하는 데이터들과 bind.
        BindChannels
        {
            Bind "Color", color
            Bind "Vertex", vertex
            Bind "TexCoord", texcoord
        }

        SubShader
        {
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                // 쉐이더 시작부의 변수들과 매핑.
                sampler2D _MainTex;
                float _Amount;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f 
                {
                    float4 pos : POSITION;
                    float4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                v2f vert(appdata v)
                {
                    // UnityObjectToClipPos : 월드 -> 클립 좌표로 변환, 그릴 위치를 결정.
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.color = v.color; // 정점의 색상 
                    o.texcoord = v.texcoord; // 정점의 텍스쳐 좌표
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    half4 texColor = tex2D(_MainTex, i.texcoord);

                    if (texColor.a <= 0.1)
                    {
                        discard;
                    }

                    half4 finalColor = lerp(texColor, half4(1, 1, 1, 1), _Amount);

                    return finalColor;
                }

                ENDCG
            }
        }
    }
}
