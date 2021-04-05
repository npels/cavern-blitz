using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveGenerator {

    #region Static variables
    private static float ROUGHNESS_MULTIPLIER = 0.05f;
    private static float FALLOFF_MULTIPLIER = 1f;
    #endregion

    #region Generation variables
    GenerationSettings settings;
    Noise noiseFilter;
    #endregion

    #region Constructors
    public CaveGenerator(GenerationSettings settings) {
        this.settings = settings;
        noiseFilter = new Noise(settings.seed);
    }
    #endregion

    #region Generation functions
    public float IsWallAtPoint(float x, float y) {
        Vector3 point = new Vector3(x, y, 0);
        float totalValue = 0;

        for (int i = 0; i < settings.noiseFilterSettings.Count; i++) {
            GenerationSettings.NoiseFilterSettings noiseSettings = settings.noiseFilterSettings[i];

            float value = noiseFilter.Evaluate(point * noiseSettings.roughness * ROUGHNESS_MULTIPLIER);

            value = (value + 1) * 0.5f;
            value = value * noiseSettings.strength;
            value = value * (1 - point.magnitude * noiseSettings.falloff * FALLOFF_MULTIPLIER / settings.caveRadius);

            totalValue += value;
        }

        return totalValue;
    }
    #endregion
}
