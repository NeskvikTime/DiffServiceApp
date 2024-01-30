using DiffServiceApp.Application.Common.Interfaces;

namespace DiffServiceApp.Application.Services;
public class DiffPositionalProcessor(IDiffPayloadsRepository _diffPayloadRepository) : IDiffPositionalProcessor
{
    private byte[] leftData;

    private byte[] rightData;

    public void SetLeftData(int id, byte[] data)
    {

        leftData = data;
    }

    public void SetRightData(int id, byte[] data)
    {


    }

    public object GetDiffResult(string id)
    {
        if (!dataStore.ContainsKey(id) || dataStore[id].Left == null || dataStore[id].Right == null)
        {
            return new { diffResultType = "Not Found" };
        }

        var (left, right) = dataStore[id];

        if (left.SequenceEqual(right))
        {
            return new { diffResultType = "Equals" };
        }

        if (left.Length != right.Length)
        {
            return new { diffResultType = "SizeDoNotMatch" };
        }

        var diffs = new List<object>();
        for (int i = 0; i < left.Length; i++)
        {
            if (left[i] != right[i])
            {
                int length = 1;
                while (i + length < left.Length && left[i + length] != right[i + length])
                {
                    length++;
                }

                diffs.Add(new { offset = i, length = length });
                i += length - 1;
            }
        }

        return new { diffResultType = "ContentDoNotMatch", diffs = diffs };
    }
}
