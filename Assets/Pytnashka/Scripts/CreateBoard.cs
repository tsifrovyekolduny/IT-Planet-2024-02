using UnityEngine;

public class CreateBoard : MonoBehaviour
{
    public static float alpha_value = 0.0f;
    public static Vector3 komod_end_location = new Vector3(4.46999979f, 0.810000002f, 0.49000001f);
    public static Vector3 komod_start_location = new Vector3(5.44000006f, 2.28999996f, 0.49000001f);
    public static Vector3 kamera_end_location = new Vector3(1.60000002f, 8.97000027f, 0.540000021f);
    public static Vector3 kamera_start_location = new Vector3(-0.5f, 6.0f, 0.540000021f);

    public float percent_anim_compeate{
        get
        {
            return alpha_value/1.0f;
        }
    }
    public GameObject[] chips;
    private int skiped_element;


    public Vector3 board_position = new Vector3(-2f, -10f, -2f);
    public GameObject ramka, kartinka, oboi, comode, kamera;
    
    //private static int SP_Alpha_Value = Shader.PropertyToID("_CustomAlphaValue");

    void Start()
    {
        try
        {
            Global.ramka = ramka;
            Global.kartinka = kartinka;
            Global.oboi = oboi;
            Global.comod = comode;
            Global.camera = kamera;
            GenerateBoard();
            ShowBoard();

            Renderer renderer = Global.kartinka.GetComponent<Renderer>();
            var matetial = renderer.material;

            renderer = Global.oboi.GetComponent<Renderer>();
            var color = renderer.material.color;
            color.a = alpha_value; // 1f делает объект полностью непрозрачным
            renderer.material.color = color;
        }
        catch
        {
            Debug.Log("Что-то не так");
        }
    }
    void Update()
    {
        if (MoveChip.game_finished)
        {
            if (alpha_value < 1)
            {
                alpha_value += 0.005f;

                //Картинка
                Renderer renderer = Global.kartinka.GetComponent<Renderer>();
                Color color = renderer.material.color;
                color.a = alpha_value; // 1f делает объект полностью непрозрачным
                renderer.material.color = color;

                //Обои
                renderer = Global.oboi.GetComponent<Renderer>();
                color = renderer.material.color;
                color.a = alpha_value; // 1f делает объект полностью непрозрачным
                renderer.material.color = color;

                //Комод
                Transform komod = Global.comod.GetComponent<Transform>();
                //Vector3 pos = komod.position;
                Vector3 directionVector = komod_end_location - komod_start_location;
                directionVector*=percent_anim_compeate;
                Vector3 end_location = komod_start_location + directionVector;
                komod.position = end_location;

                //Камера
                Transform kamera = Global.camera.GetComponent<Transform>();
                directionVector = kamera_end_location - kamera_start_location;
                directionVector *= percent_anim_compeate;
                end_location = kamera_start_location + directionVector;
                kamera.position = end_location;

            }
        }

    }
    void GenerateBoard()
    {
        if (!Global.test)
        {
            int maxCountBlocks = 16 - Global.countEraceBlocks;
            for (int i = 0; i <= maxCountBlocks; i++)
            {
                bool cancel = false;
                do
                {
                    int row = Random.Range(0, 4);
                    int col = Random.Range(0, 4);
                    if (Global.board[row + Global.x_offset, col + Global.y_offset] == 0)
                    {
                        cancel = true;
                        Global.board[row + Global.x_offset, col + Global.y_offset] = i;
                    }
                } while (!cancel);
            }
        }
        else
        {
            int maxCountBlocks = 16 - Global.countEraceBlocks;
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    Global.board[row + Global.x_offset, col + Global.y_offset] = row * 4 + col + 1;
                }
            }
        }
    }
    void ShowBoard()
    {
        GlobalVars.chips = chips;

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (Global.board[row + Global.x_offset, col + Global.y_offset] != 0)
                {
                    Vector3 coardinate = new Vector3(board_position.x + row * GlobalVars.x_offset, board_position.y, board_position.z + col * GlobalVars.z_offset);
                    int chip = Global.board[row + Global.x_offset, col + Global.y_offset] - 1;
                    var obj = Instantiate(chips[chip], coardinate, transform.rotation);
                    obj.name = chips[chip].name;
                }
            }
        }
    }
}
