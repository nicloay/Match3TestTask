using System;


namespace Match3.Utils
{
    public static class ArrayUtil
    {

        /// <summary>
        /// Generate sequnces like for sequence 0->3 and total number 2 you will get [0,1,2,3,0,1,2,3]
        /// </summary>
        /// <param name="sequenceSize">total numbers in one sequnce</param>
        /// <param name="sequenceNumber">total sequences number</param>
        /// <returns></returns>
        public static int[] GenerateSequencedArray(int sequenceSize, int sequenceNumber)
        {
            int[] result = new int[sequenceSize * sequenceNumber];
            int i = 0;
            for (int sequenceId = 0; sequenceId < sequenceNumber; sequenceId++)
            {
                for (int numberId = 0; numberId < sequenceSize; numberId++)
                {
                    result[i++] = numberId;
                }
            }

            return result;
        }

        public static void ShuffleArray(int[] array, Random random)
        {
            int tempValue, tempId;
            for (int i = 0; i < array.Length; i++)
            {
                tempValue = array[i];
                tempId = random.Next(array.Length);
                array[i] = array[tempId];
                array[tempId] = tempValue;
            }
        }

        public static T[,] ConvertArrayFromHumanReadebleFormatToNative<T>(T[,] source)
        {
            int sourceWidth = source.GetLength(0);
            int sourceHeight = source.GetLength(1);
            int targetWidth = sourceHeight;
            int targetHeight = sourceWidth;
            T[,] result = new T[targetWidth, targetHeight];

            for (int y = 0; y < targetHeight; y++)
            {
                for (int x = 0; x < targetWidth; x++)
                {
                    result[x, y] = source[sourceWidth - y - 1, x];
                }
            }            
            return result;
        }
    }
}