#version 330

#ifdef GL_ES
precision mediump float;
#endif

out vec4 FragColor;

in vec2 texCoords; // X,Y
in vec4 backColor; // RGBA
in vec4 foreColor; // RGBA

uniform sampler2D atlasTexture;

void main() {
//    vec4 mask = texture(atlasTexture, texCoords);
//    float maskAlpha = mask.x * mask.w;
//
//    vec4 finalColor = foreColor * maskAlpha;
//    finalColor = (foreColor * foreColor.w) + (backColor * (1 - foreColor.w));

    //FragColor = finalColor;
    FragColor = vec4(1.0, 0.0, 0.0, 1.0);
}