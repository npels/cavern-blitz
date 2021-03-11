using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveGenerator {

    private static float ROUGHNESS_MULTIPLIER = 0.05f;
    private static float FALLOFF_MULTIPLIER = 1f;

    GenerationSettings settings;
    NoiseFilter noiseFilter;

    public CaveGenerator(GenerationSettings settings) {
        this.settings = settings;
        noiseFilter = new NoiseFilter(settings.seed);
    }

    public float IsWallAtPoint(float x, float y) {
        Vector3 point = new Vector3(x, y, 0);
        float value = noiseFilter.Evaluate(point * settings.roughness * ROUGHNESS_MULTIPLIER);

        value = value * (1 - point.magnitude * settings.falloff * FALLOFF_MULTIPLIER / settings.caveRadius);

        return value;
    }
}
