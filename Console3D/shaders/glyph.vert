#version 330

#ifdef GL_ES
precision mediump float;
#endif

layout (location = 0) in vec2 apos; // X,Y
layout (location = 1) in vec2 atex; // X,Y
layout (location = 2) in vec4 aback; // RGBA
layout (location = 3) in vec4 afore; // RGBA

uniform vec4 resolution; // X,Y,W,H
uniform sampler2D atlasTexture; // Atlas

out vec2 texCoords; // X,Y
out vec4 backColor; // RGBA
out vec4 foreColor; // RGBA

void main() {
    gl_Position = vec4(((apos.x - resolution.x) * 2) / resolution.z, ((apos.y - resolution.y) * 2) / resolution.w, 0.0, 1.0);
    gl_Position.x -= 1;
    gl_Position.y -= 1;
    gl_Position.y *= -1;

    vec2 atlSize = textureSize(atlasTexture, 0);
    texCoords = vec2(atex.x / atlSize.x, atex.y / atlSize.y);
    texCoords.y *= -1;

    backColor = aback;
    foreColor = afore;
}