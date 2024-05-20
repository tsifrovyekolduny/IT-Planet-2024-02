using UnityEngine;
using UnityEngine.UI;

public class CreateBoard : MonoBehaviour
{
    public static Vector3 komod_end_location = new Vector3(4.46515036f, -0.560000002f, 0.49000001f);
    public static Vector3 komod_start_location = new Vector3(5.44000006f, 2.28999996f, 0.49000001f);
    public static Vector3 kamera_end_location = new Vector3(1.60000002f, 8.97000027f, 0.540000021f);
    public static Vector3 kamera_end_location_over = new Vector3(1.60000002f, 1000.97000027f, 0.540000021f);
    public static Vector3 kamera_start_location = new Vector3(-0.5f, 6.0f, 0.540000021f);

    public float percent_anim_compeate{
        get
        {
            return Global.Get.alpha_value/1.0f;
        }
    }
    public GameObject[] chips;


    public Vector3 board_position = new Vector3(-2f, -10f, -2f);
    public GameObject ramka, kartinka, oboi, comode, kamera;
    public Text count_steps;
    
    //private static int SP_Alpha_Value = Shader.PropertyToID("_CustomAlphaValue");

    void Start()
    {
        Global.ResetInstance();
        try
        {
            Global.Get.ramka = ramka;
            Global.Get.kartinka = kartinka;
            Global.Get.oboi = oboi;
            Global.Get.comod = comode;
            Global.Get.camera = kamera;
            Global.Get.count_steps = count_steps;
            GenerateBoard();
            ShowBoard();
            Global.Get.UpdateText();


            Renderer renderer = Global.Get.kartinka.GetComponent<Renderer>();
            var matetial = renderer.material;

            renderer = Global.Get.oboi.GetComponent<Renderer>();
            var color = renderer.material.color;
            color.a = Global.Get.alpha_value; // 1f делает объект полностью непрозрачным
            renderer.material.color = color;

            Global.Get.ramka.SetActive(false);
            Global.Get.oboi.SetActive(false);
            Global.Get.kartinka.SetActive(false);

            Global.Get.ramka.GetComponent<MeshRenderer>().enabled = false;
            Global.Get.kartinka.GetComponent<SpriteRenderer>().enabled = false;
            Global.Get.comod.GetComponent<MeshRenderer>().enabled = false;
            Global.Get.count_steps.enabled = true;
            Global.Get.oboi.GetComponent<MeshRenderer>().enabled = false;
        }
        catch
        {
            Debug.Log("Что-то не так");
        }
    }
    void Update()
    {
        if (Global.Get.game_finished)
        {
            Global.Get.ramka.SetActive(true);
            Global.Get.oboi.SetActive(true);
            Global.Get.kartinka.SetActive(true);

            if (Global.Get.alpha_value < 1)
            {
                Global.Get.alpha_value += 0.005f;

                //Картинка
                Renderer renderer = Global.Get.kartinka.GetComponent<Renderer>();
                Color color = renderer.material.color;
                color.a =  Global.Get.alpha_value; // 1f делает объект полностью непрозрачным
                renderer.material.color = color;

                //Обои
                renderer = Global.Get.oboi.GetComponent<Renderer>();
                color = renderer.material.color;
                color.a = Global.Get.alpha_value; // 1f делает объект полностью непрозрачным
                renderer.material.color = color;

                //Комод
                Transform komod = Global.Get.comod.GetComponent<Transform>();
                //Vector3 pos = komod.position;
                Vector3 directionVector = komod_end_location - komod_start_location;
                directionVector*=percent_anim_compeate;
                Vector3 end_location = komod_start_location + directionVector;
                komod.position = end_location;

                //Камера
                Transform kamera = Global.Get.camera.GetComponent<Transform>();
                directionVector = kamera_end_location - kamera_start_location;
                directionVector *= percent_anim_compeate;
                end_location = kamera_start_location + directionVector;
                kamera.position = end_location;

            }
            else
            {
                if (Global.Get.is_game_over)
                {
                    //Поднимаем камеру
                    //Камера
                    Transform kamera = Global.Get.camera.GetComponent<Transform>();
                    if(kamera.position.y < kamera_end_location_over.y)
                    {
                       Vector3 new_pos = kamera.position;
                       new_pos.y += 1f;
                       kamera.position = new_pos;
                    }
                }
            }
        }

    }
    void GenerateBoard()
    {
        if (!Global.Get.test)
        {
            int maxCountBlocks = 16 - Global.Get.countEraceBlocks;
            for (int i = 0; i <= maxCountBlocks; i++)
            {
                bool cancel = false;
                do
                {
                    int row = Random.Range(0, 4);
                    int col = Random.Range(0, 4);
                    if (Global.Get.board[row + Global.x_offset, col + Global.y_offset] == 0)
                    {
                        cancel = true;
                        Global.Get.board[row + Global.x_offset, col + Global.y_offset] = i;
                    }
                } while (!cancel);
            }
        }
        else
        {
            int maxCountBlocks = 16 - Global.Get.countEraceBlocks;
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    Global.Get.board[row + Global.x_offset, col + Global.y_offset] = row * 4 + col + 1;
                }
            }
        }
    }
    void ShowBoard()
    {
        Global.Get.chips = chips;

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (Global.Get.board[row + Global.x_offset, col + Global.y_offset] != 0)
                {
                    Vector3 coardinate = new Vector3(board_position.x + row * Global.Get.x_offset_f, board_position.y, board_position.z + col * Global.Get.z_offset_f);
                    int chip = Global.Get.board[row + Global.x_offset, col + Global.y_offset] - 1;
                    var obj = Instantiate(chips[chip], coardinate, transform.rotation);
                    obj.name = chips[chip].name;
                }
            }
        }
    }
}
