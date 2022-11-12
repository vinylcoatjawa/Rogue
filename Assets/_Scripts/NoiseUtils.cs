using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NoiseUtils
{
    /// <summary>
    /// Various noise based random generators
    /// </summary>
    public class Noise
    {
        //const uint _noise1 = 0b11010111010101010010011111101011;
        //const uint _noise2 = 0b11101101101010101100000101101101;
        //const uint _noise3 = 0b11000101010100111101000110001011;
        const uint _noise1 = 0xB5297A4D;
        const uint _noise2 = 0x68E31DA4;
        const uint _noise3 = 0x1B56C4E9;
        const uint _prime = 0b11101110011010110010101101011101;

        /****************         1D          ***********************/

        /// <summary>
        /// Get the noise functions value on a specified place, shifted by a seed
        /// </summary>
        /// <param name="position">The input of the noise generator</param>
        /// <param name="seed">Seed parameter of the random generator</param>
        /// <returns>A random uint based on the position and the seed</returns>
        public uint Get1DNoiseUint(uint position, uint seed)
        {
            uint mangled = position;
            mangled *= _noise1;
            mangled += seed;
            mangled ^= (mangled << 8);
            mangled += _noise2;
            mangled ^= (mangled >> 8);
            mangled *= _noise3;
            mangled ^= (mangled << 8);

            return mangled;
        }
        /// <summary>
        /// Get the noise functions value on a specified place, shifted by a seed then mods it so it ends up in a range
        /// </summary>
        /// <param name="rangeFrom"></param>
        /// <param name="rangeTo"></param>
        /// <param name="position"></param>
        /// <param name="seed"></param>
        /// <returns>A random uint in the a given range</returns>
        public uint NoiseInRange(uint rangeFrom, uint rangeTo, uint position, uint seed)
        {
            uint mangled = Get1DNoiseUint(position, seed);
            return rangeFrom + mangled % (rangeTo - rangeFrom);
        }
        /// <summary>
        /// Gets a float between 0 and 1 based on the noise functions value on a specified place, shifted by a seed 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="seed"></param>
        /// <returns>Float between 0 and 1</returns>
        public float Get1DNoiseZeroToOne(uint position, uint seed)
        {
            uint mangled = position;
            mangled *= _noise1;
            mangled += seed;
            mangled ^= (mangled << 8);
            mangled += _noise2;
            mangled ^= (mangled >> 8);
            mangled *= _noise3;
            mangled ^= (mangled << 8);

            return (float)mangled / uint.MaxValue;
        }
        /// <summary>
        /// Get the noise functions value on a specified place, shifted by a seed modded by two
        /// </summary>
        /// <param name="position"></param>
        /// <param name="seed"></param>
        /// <returns>Returns 0 or 1</returns>
        public uint ZeroOrOne(uint position, uint seed)
        {
            uint mangled = position;
            mangled *= _noise1;
            mangled += seed;
            mangled ^= (mangled << 8);
            mangled += _noise2;
            mangled ^= (mangled >> 8);
            mangled *= _noise3;
            mangled ^= (mangled << 8);

            return (uint)Mathf.RoundToInt((float)mangled / uint.MaxValue);
        }
        /// <summary>
        /// Get the noise functions value on a specified place, shifted by a seed modded by 3
        /// </summary>
        /// <param name="position"></param>
        /// <param name="seed"></param>
        /// <returns>-1, 0 or 1</returns>
        public int ZeroOrOneOrMinusOne(uint position, uint seed)
        {
            uint mangled = position;
            mangled *= _noise1;
            mangled += seed;
            mangled ^= (mangled << 8);
            mangled += _noise2;
            mangled ^= (mangled >> 8);
            mangled *= _noise3;
            mangled ^= (mangled << 8);

            return (int)(mangled % 3) - 1;
        }


        /****************        2D              **************************/
        /// <summary>
        /// Get the noise functions value on a specified 2D coordinate, shifted by a seed modded by two
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="seed"></param>
        /// <returns>A random uint based on 2D coordinates</returns>
        public uint Get2DNoiseUint(uint posX, uint posY, uint seed)
        {
            return Get1DNoiseUint(posX + (_prime * posY), seed);
        }
        /// <summary>
        /// Gets a float between 0 and 1 based on the noise functions value on a specified 2D coordinate, shifted by a seed
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public float Get2DNoiseZeroToOne(uint posX, uint posY, uint seed)
        {
            return Get1DNoiseZeroToOne(posX + (_prime * posY), seed);
        }
    }
}

