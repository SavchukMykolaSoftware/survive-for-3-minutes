public static class RandomNumbersGenerator
{
    public static float GenerateRandomFloatNumber(float minimalPossibleNumber, float maximalPossibleNumber)
    {
        return ((float)new System.Random().NextDouble()) * (maximalPossibleNumber - minimalPossibleNumber) + minimalPossibleNumber;
    }
}