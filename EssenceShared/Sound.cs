using System;

namespace EssenceShared {
    public class Sound {
        public static bool IsPause = false;
        public static int MasterVol { get; set; }

        /// <summary>
        ///     Воспроизводит музыку на фоне в формате midi, ogg или mp3. Если музыка уже играет, она плавно затухает
        ///     и новая начинает плавно играть ( с низкой громокости)
        /// </summary>
        public static void PlayBackgroundMusic(string path, int vol = 100) {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Воспроизводит разово эффект в формате .wav
        /// </summary>
        public static void PlayEffect(string path, int vol = 100) {
            throw new NotImplementedException();
        }

        public static void Mute() {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Ставит фоновую музыку на паузу (тоже плавно)
        /// </summary>
        public static void Pause() {
            throw new NotImplementedException();
        }
    }
}