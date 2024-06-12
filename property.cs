public class volumeInfo{
    public float megaBytes{
        get{ return m_bytes * 0.000001f; }
        set{
            if(valsue <= 0){
                m_bytes = 0;
            }else{
                m_bytes = value * 100000f;
            }
        }
    }

    public float killoBytes{
        get{ return m_bytes * 0.001f; }
        set{
            if(valsue <= 0){
                m_bytes = 0;
            }else{
                m_bytes = value * 1000f;
            }
        }
    }

    public float bytes{
        get{ return m_bytes; }
        set{
            if(valsue <= 0){
                m_bytes = 0;
            }else{
                m_bytes = value;
            }
        }
    }

    private float m_bytes = 0;

    volumeInfo info = new volumeInfo();

    volumeInfo.bytes = 100000;
    Debug.Log(info.killoBytes);
    Debug.Log(info.megaBytes);

    info.megaBytes = 4;
    Debug.Log(info.bytes);
}