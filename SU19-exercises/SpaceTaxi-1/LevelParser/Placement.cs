using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SpaceTaxi_1 {
    public class Placement {


        private static Tuple<float, float> CalculateMapSize(string[] text) {
            return (new Tuple<float, float>((float) 1/text[0].Length, (float) 1/text.Length));

        }

        private static Tuple<float, float> Convert(int x, int y, Tuple<float, float> map) {
            return new Tuple<float, float>((float) x*map.Item1,(float) y*map.Item2);

        }


        public static List<Tuple<float, float>> FindPlacement(string[] text) {
            Tuple<float, float> mapsize = CalculateMapSize(text);  
            List<Tuple<float, float>> places = new List<Tuple<float, float>>();
            
            for (int y = 0; y < text.Length-1; y++) {
                for (int x = 0; x < text[y].Length; x++) {
                    if (text[y][x] != ' ') {
                        places.Add(Convert(x, y, mapsize));

                    }


                }
            }

            return places;
        }
        }
        
    }