shader_type canvas_item;

vec2 rotateUV(vec2 uv, vec2 pivot, float rotation) {
    float sine = sin(rotation);
    float cosine = cos(rotation);
    
    uv -= pivot;
	vec2 customvec = vec2(uv.x * cosine - uv.y * sine, uv.x * sine + uv.y * cosine);
    uv = customvec;
    uv += pivot;

    return uv;uv.x = uv.x * cosine - uv.y * sine;
    uv.y = uv.x * sine + uv.y * cosine;
}

void vertex() {
	UV = rotateUV(UV, vec2(0.5), TIME*-0.25);
}

void fragment() {
}
