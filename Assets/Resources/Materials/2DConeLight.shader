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

                // ���̴����� ����� ��� ������ ���⼭ �����մϴ�.
                float4 _Position;
                float4 _Color;
                float _Angle;
                float _Intensity;

                sampler2D _MainTex;

                // ����ü ���� - ��ȯ �� �ؽ�ó ��ǥ ó����
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

                // ���ؽ� ���̴�
                v2f vert(appdata v)
                {
                    v2f o;
                    o.position = UnityObjectToClipPos(v.position);
                    o.texcoord = v.texcoord;
                    return o;
                }

                // �����׸�Ʈ ���̴�
                half4 frag(v2f i) : SV_Target
                {
                    // ���� ��ġ�� �ؽ�ó ��ǥ�� ������� ���� ȿ�� ���
                    float2 lightPos = _Position.xy;
                    float2 toLight = i.texcoord - lightPos;

                    // ���� ���
                    float distance = length(toLight);
                    float coneFactor = step(distance / tan(radians(_Angle)), 1.0);

                    // �ؽ�ó ����� ���� ����
                    half4 texColor = tex2D(_MainTex, i.texcoord) * _Intensity;

                    // ���� ���� ���� ���� ���� ��ȯ
                    return texColor * coneFactor;
                }

                ENDCG
            }
        }

 FallBack "Diffuse"
}