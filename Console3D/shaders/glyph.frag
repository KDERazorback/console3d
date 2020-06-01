#version 330

#ifdef GL_ES
precision mediump float;
#endif

out vec4 FragColor;

noperspective in vec2 texCoords; // X,Y
in vec4 backColor; // RGBA
in vec4 foreColor; // RGBA

uniform sampler2D atlasTexture;

vec4 over(vec4 ca, vec4 cb)
{
    float aa = ca.w;
    float ab = cb.w;

    ca *= aa;
    cb *= ab;

    vec4 res = ca + (cb * (1 - aa));
    res.w = aa + (ab * (1 - aa));

    return res;
}

void main() {
    vec2 coord = vec2(texCoords);
    vec4 mask = texture(atlasTexture, coord);
    float maskAlpha = 1 - (mask.x * mask.w);

    vec4 finalColor = vec4(foreColor);
    finalColor.w *= maskAlpha;

    vec4 color = over(finalColor, backColor);

    if (color.w == 0)
        discard;

    FragColor = color;
}