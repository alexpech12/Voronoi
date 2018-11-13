/****************************************************
 * https://en.wikipedia.org/wiki/LU_decomposition
 * **************************************************/

public static class SystemOfLinearEquations
{
    public static double[] SolveUsingLU(double[,] matrix, double[] rightPart, int n)
    {
        // decomposition of matrix
        double[,] lu = new double[n, n];
        double sum = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = i; j < n; j++)
            {
                sum = 0;
                for (int k = 0; k < i; k++)
                    sum += lu[i, k] * lu[k, j];
                lu[i, j] = matrix[i, j] - sum;
            }
            for (int j = i + 1; j < n; j++)
            {
                sum = 0;
                for (int k = 0; k < i; k++)
                    sum += lu[j, k] * lu[k, i];
                lu[j, i] = (1 / lu[i, i]) * (matrix[j, i] - sum);
            }
        }

        // find solution of Ly = b
        double[] y = new double[n];
        for (int i = 0; i < n; i++)
        {
            sum = 0;
            for (int k = 0; k < i; k++)
                sum += lu[i, k] * y[k];
            y[i] = rightPart[i] - sum;
        }
        // find solution of Ux = y
        double[] x = new double[n];
        for (int i = n - 1; i >= 0; i--)
        {
            sum = 0;
            for (int k = i + 1; k < n; k++)
                sum += lu[i, k] * x[k];
            x[i] = (1 / lu[i, i]) * (y[i] - sum);
        }
        return x;
    }
}
