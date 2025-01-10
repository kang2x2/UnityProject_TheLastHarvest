Shader "Custom/ColorLerp"
{
    Properties
    {
        // ������, �ͽ����Ϳ��� ������ ������Ƽ �̸�, ������Ƽ, �ʱⰪ
        _MainTex("Texture", 2D) = "white" { }
        _Amount("FadeAmount", Range(0, 1)) = 0.0
    }

    Category
    {
        // "Queue" = "Overlay" : �ش� ���̴��� ȭ�鿡 �������� ����, ���� ��ü�麸�� �켱�ؼ� ������.
        // "IgnoreProjector" = "True" : �׸���(�������� ȿ��)�� ���� ���� ����.
        // RenderType: "Transparent" : ���� ��ü�� ������, ���� ȿ���� �ִ� �ؽ��Ŀ� ����.
        Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        // Blend SrcAlpha One
        Blend SrcAlpha OneMinusSrcAlpha // ���� ���� ��� ���.
        Cull Off Lighting Off ZWrite Off Fog { Color(0, 0, 0, 0) }

        // Cull Off // ��� ������. �齺���̽� �ø��� ������� �ʰڴ�.
        // Lighting Off // ���� ó�� ���� �ؽ��Ŀ� ����� ���� ���.
        // ZWrite Off // ���� ���� off.

        // �ش� �����͵��� ���̴����� ����ϴ� �����͵�� bind.
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

                // ���̴� ���ۺ��� ������� ����.
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
                    // UnityObjectToClipPos : ���� -> Ŭ�� ��ǥ�� ��ȯ, �׸� ��ġ�� ����.
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.color = v.color; // ������ ���� 
                    o.texcoord = v.texcoord; // ������ �ؽ��� ��ǥ
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
