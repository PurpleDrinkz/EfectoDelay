using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Efectos
{
    class Delay : ISampleProvider
    {
        private ISampleProvider fuente;


        public int offsetTiempoMS;

        List<float> muestras = new List<float>();

        public Delay(ISampleProvider fuente)
        {
            this.fuente = fuente;
            ////Factor = 0.5f;
        }

        public Delay(ISampleProvider fuente, float factor)
        {
            this.fuente = fuente;
            
        }

        public WaveFormat WaveFormat
        {
            get
            {
               return fuente.WaveFormat;
            }
        }

        //Offset, numero de muestras leídas
        public int Read(float[] buffer, int offset, int count)
        {
           

            var read = fuente.Read(buffer, offset, count);
            float tiempoTranscurrido = (float)muestras.Count / (float)fuente.WaveFormat.SampleRate;
            int muestrasTranscurridas = muestras.Count;
            float tiempoTranscurridoMS = tiempoTranscurrido * 1000;
            int numMuestrasOffsetTiempo = (int)(((float)offsetTiempoMS / 1000.0f) * (float)fuente.WaveFormat.SampleRate);

            for (int i = 0; i < read; i++)
            {
                muestras.Add(buffer[i]);


            }



            if (tiempoTranscurridoMS > offsetTiempoMS)
            {
                for (int i = 0; i < read; i++)
                {

                    buffer[i] +=
                        muestras[muestrasTranscurridas + i - numMuestrasOffsetTiempo];
                    

                }
            }

            

            return read;
        }
    }
}
