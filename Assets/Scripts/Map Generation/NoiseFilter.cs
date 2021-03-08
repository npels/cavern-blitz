using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter {

    Noise noise;

    public NoiseFilter() {
        noise = new Noise();
    }

    public NoiseFilter(int seed) {
        noise = new Noise(seed);
    }

    public float Evaluate(Vector3 point) {
        float noiseValue = (noise.Evaluate(point) + 1) * 0.5f;
        return noiseValue;
    }
}
