#version 330

#ifdef GL_ES
precision medium float;
#endif

layout (location = 0) in vec2 apos; // X,Y
layout (location = 1) in vec2 atex; // X,Y
layout (location = 2) in vec4 aback; // RGBA
layout (location = 3) in vec4 afore; // RGBA

uniform vec4 resolution; // X,Y,W,H
uniform sampler2D atlasTexture; // Atlas
uniform vec2 atlasSize; // W,H

noperspective out vec2 texCoords; // X,Y
out vec4 backColor; // RGBA
out vec4 foreColor; // RGBA

void main() {
    vec4 finalPos = vec4(1.0);
    finalPos.x = apos.x - resolution.x;
    finalPos.x *= 2.0;
    finalPos.x /= resolution.z;
    finalPos.x -= 1;
    
    finalPos.y = apos.y - resolution.y;
    finalPos.y *= 2.0;
    finalPos.y /= resolution.w;
    finalPos.y -= 1;
    finalPos.y *= -1;

    gl_Position = finalPos;

    vec2 atlSize = textureSize(atlasTexture, 0);
    texCoords = vec2(atex.x / atlasSize.x, atex.y / atlasSize.y);

    backColor = aback;
    foreColor = afore;
}