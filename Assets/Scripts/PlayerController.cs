using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class PlayerController : MonoBehaviour
{
    private NetworkStream stream;
    private bool isHost;

    public void Init(NetworkStream stream, bool isHost)
    {
        this.stream = stream;
        this.isHost = isHost;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal") * Time.deltaTime * 5f;
        float v = Input.GetAxis("Vertical") * Time.deltaTime * 5f;
        transform.Translate(new Vector3(h, 0, v));

        if (stream != null && stream.CanWrite)
        {
            string posData = transform.position.ToString();
            byte[] data = Encoding.ASCII.GetBytes(posData);
            stream.Write(data, 0, data.Length);
        }
    }
}
