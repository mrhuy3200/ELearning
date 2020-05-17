/*************************************************************
 *
 *  Copyright (c) 2018 The MathJax Consortium
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */

import {CharMap, CharOptions} from '../../FontData.js';

export const bold: CharMap<CharOptions> = {
    0x21: [.705, 0, .35],
    0x22: [.694, -0.329, .603],
    0x23: [.694, .193, .958],
    0x24: [.75, .056, .575],
    0x25: [.75, .056, .958],
    0x26: [.705, .011, .894],
    0x27: [.694, -0.329, .319],
    0x28: [.75, .249, .447],
    0x29: [.75, .249, .447],
    0x2A: [.75, -0.306, .575],
    0x2B: [.633, .131, .894],
    0x2C: [.171, .194, .319],
    0x2D: [.278, -0.166, .383],
    0x2E: [.171, 0, .319],
    0x2F: [.75, .25, .575],
    0x3A: [.444, 0, .319],
    0x3B: [.444, .194, .319],
    0x3C: [.587, .085, .894],
    0x3D: [.393, -0.109, .894],
    0x3E: [.587, .085, .894],
    0x3F: [.7, 0, .543],
    0x40: [.699, .006, .894],
    0x5B: [.75, .25, .319],
    0x5C: [.75, .25, .575],
    0x5D: [.75, .25, .319],
    0x5E: [.694, -0.52, .575],
    0x5F: [-0.01, .061, .575],
    0x60: [.706, -0.503, .575],
    0x7B: [.75, .25, .575],
    0x7C: [.75, .249, .319],
    0x7D: [.75, .25, .575],
    0x7E: [.344, -0.202, .575],
    0xA8: [.695, -0.535, .575],
    0xAC: [.371, -0.061, .767],
    0xAF: [.607, -0.54, .575],
    0xB0: [.702, -0.536, .575],
    0xB1: [.728, .035, .894],
    0xB4: [.706, -0.503, .575],
    0xB7: [.336, -0.166, .319],
    0xD7: [.53, .028, .894],
    0xF7: [.597, .096, .894],
    0x131: [.442, 0, .278, {sk: .0278}],
    0x237: [.442, .205, .306, {sk: .0833}],
    0x2B9: [.563, -0.033, .344],
    0x2C6: [.694, -0.52, .575],
    0x2C7: [.66, -0.515, .575],
    0x2C9: [.607, -0.54, .575],
    0x2CA: [.706, -0.503, .575],
    0x2CB: [.706, -0.503, .575],
    0x2D8: [.694, -0.5, .575],
    0x2D9: [.695, -0.525, .575],
    0x2DA: [.702, -0.536, .575],
    0x2DC: [.694, -0.552, .575],
    0x300: [.706, -0.503, 0],
    0x301: [.706, -0.503, 0],
    0x302: [.694, -0.52, 0],
    0x303: [.694, -0.552, 0],
    0x304: [.607, -0.54, 0],
    0x306: [.694, -0.5, 0],
    0x307: [.695, -0.525, 0],
    0x308: [.695, -0.535, 0],
    0x30A: [.702, -0.536, 0],
    0x30B: [.714, -0.511, 0],
    0x30C: [.66, -0.515, 0],
    0x338: [.711, .21, 0],
    0x2002: [0, 0, .5],
    0x2003: [0, 0, .999],
    0x2004: [0, 0, .333],
    0x2005: [0, 0, .25],
    0x2006: [0, 0, .167],
    0x2009: [0, 0, .167],
    0x200A: [0, 0, .083],
    0x2013: [.3, -0.249, .575],
    0x2014: [.3, -0.249, 1.15],
    0x2015: [.3, -0.249, 1.15],
    0x2016: [.75, .248, .575],
    0x2017: [-0.01, .061, .575],
    0x2018: [.694, -0.329, .319],
    0x2019: [.694, -0.329, .319],
    0x201C: [.694, -0.329, .603],
    0x201D: [.694, -0.329, .603],
    0x2020: [.702, .211, .511],
    0x2021: [.702, .202, .511],
    0x2022: [.474, -0.028, .575],
    0x2026: [.171, 0, 1.295],
    0x2032: [.563, -0.033, .344],
    0x2033: [.563, 0, .688],
    0x2034: [.563, 0, 1.032],
    0x203E: [.607, -0.54, .575],
    0x2044: [.75, .25, .575],
    0x2057: [.563, 0, 1.376],
    0x20D7: [.723, -0.513, .575],
    0x210F: [.694, .008, .668, {sk: -0.0319}],
    0x2113: [.702, .019, .474, {sk: .128}],
    0x2118: [.461, .21, .74],
    0x2135: [.694, 0, .703],
    0x2190: [.518, .017, 1.15],
    0x2191: [.694, .193, .575],
    0x2192: [.518, .017, 1.15],
    0x2193: [.694, .194, .575],
    0x2194: [.518, .017, 1.15],
    0x2195: [.767, .267, .575],
    0x2196: [.724, .194, 1.15],
    0x2197: [.724, .193, 1.15],
    0x2198: [.694, .224, 1.15],
    0x2199: [.694, .224, 1.15],
    0x219A: [.711, .21, 1.15],
    0x219B: [.711, .21, 1.15],
    0x21A6: [.518, .017, 1.15],
    0x21A9: [.518, .017, 1.282],
    0x21AA: [.518, .017, 1.282],
    0x21AE: [.711, .21, 1.15],
    0x21BC: [.518, -0.22, 1.15],
    0x21BD: [.281, .017, 1.15],
    0x21C0: [.518, -0.22, 1.15],
    0x21C1: [.281, .017, 1.15],
    0x21CC: [.718, .017, 1.15],
    0x21CD: [.711, .21, 1.15],
    0x21CE: [.711, .21, 1.15],
    0x21CF: [.711, .21, 1.15],
    0x21D0: [.547, .046, 1.15],
    0x21D1: [.694, .193, .703],
    0x21D2: [.547, .046, 1.15],
    0x21D3: [.694, .194, .703],
    0x21D4: [.547, .046, 1.15],
    0x21D5: [.767, .267, .703],
    0x2200: [.694, .016, .639],
    0x2203: [.694, 0, .639],
    0x2204: [.711, .21, .639],
    0x2205: [.767, .073, .575],
    0x2206: [.698, 0, .958],
    0x2208: [.587, .086, .767],
    0x2209: [.711, .21, .767],
    0x220B: [.587, .086, .767],
    0x220C: [.711, .21, .767],
    0x2212: [.281, -0.221, .894],
    0x2213: [.537, .227, .894],
    0x2215: [.75, .25, .575],
    0x2216: [.75, .25, .575],
    0x2217: [.472, -0.028, .575],
    0x2218: [.474, -0.028, .575],
    0x2219: [.474, -0.028, .575],
    0x221A: [.82, .18, .958, {ic: .03}],
    0x221D: [.451, .008, .894],
    0x221E: [.452, .008, 1.15],
    0x2220: [.714, 0, .722],
    0x2223: [.75, .249, .319],
    0x2224: [.75, .249, .319],
    0x2225: [.75, .248, .575],
    0x2226: [.75, .248, .575],
    0x2227: [.604, .017, .767],
    0x2228: [.604, .016, .767],
    0x2229: [.603, .016, .767],
    0x222A: [.604, .016, .767],
    0x222B: [.711, .211, .569, {ic: .063}],
    0x223C: [.391, -0.109, .894],
    0x2240: [.583, .082, .319],
    0x2241: [.711, .21, .894],
    0x2243: [.502, 0, .894],
    0x2244: [.711, .21, .894],
    0x2245: [.638, .027, .894],
    0x2247: [.711, .21, .894],
    0x2248: [.524, -0.032, .894],
    0x2249: [.711, .21, .894],
    0x224D: [.533, .032, .894],
    0x2250: [.721, -0.109, .894],
    0x2260: [.711, .21, .894],
    0x2261: [.505, 0, .894],
    0x2262: [.711, .21, .894],
    0x2264: [.697, .199, .894],
    0x2265: [.697, .199, .894],
    0x226A: [.617, .116, 1.15],
    0x226B: [.618, .116, 1.15],
    0x226D: [.711, .21, .894],
    0x226E: [.711, .21, .894],
    0x226F: [.711, .21, .894],
    0x2270: [.711, .21, .894],
    0x2271: [.711, .21, .894],
    0x227A: [.585, .086, .894],
    0x227B: [.586, .086, .894],
    0x2280: [.711, .21, .894],
    0x2281: [.711, .21, .894],
    0x2282: [.587, .085, .894],
    0x2283: [.587, .086, .894],
    0x2284: [.711, .21, .894],
    0x2285: [.711, .21, .894],
    0x2286: [.697, .199, .894],
    0x2287: [.697, .199, .894],
    0x2288: [.711, .21, .894],
    0x2289: [.711, .21, .894],
    0x228E: [.604, .016, .767],
    0x2291: [.697, .199, .894],
    0x2292: [.697, .199, .894],
    0x2293: [.604, 0, .767],
    0x2294: [.604, 0, .767],
    0x2295: [.632, .132, .894],
    0x2296: [.632, .132, .894],
    0x2297: [.632, .132, .894],
    0x2298: [.632, .132, .894],
    0x2299: [.632, .132, .894],
    0x22A2: [.693, 0, .703],
    0x22A3: [.693, 0, .703],
    0x22A4: [.694, 0, .894],
    0x22A5: [.693, 0, .894],
    0x22A8: [.75, .249, .974],
    0x22AC: [.711, .21, .703],
    0x22AD: [.75, .249, .974],
    0x22C4: [.523, .021, .575],
    0x22C5: [.336, -0.166, .319],
    0x22C6: [.502, 0, .575],
    0x22C8: [.54, .039, 1],
    0x22E2: [.711, .21, .894],
    0x22E3: [.711, .21, .894],
    0x22EE: [.951, .029, .319],
    0x22EF: [.336, -0.166, 1.295],
    0x22F1: [.871, -0.101, 1.323],
    0x2308: [.75, .248, .511],
    0x2309: [.75, .248, .511],
    0x230A: [.749, .248, .511],
    0x230B: [.749, .248, .511],
    0x2322: [.405, -0.108, 1.15],
    0x2323: [.392, -0.126, 1.15],
    0x2329: [.75, .249, .447],
    0x232A: [.75, .249, .447],
    0x25B3: [.711, 0, 1.022],
    0x25B5: [.711, 0, 1.022],
    0x25B9: [.54, .039, .575],
    0x25BD: [.5, .21, 1.022],
    0x25BF: [.5, .21, 1.022],
    0x25C3: [.539, .038, .575],
    0x25EF: [.711, .211, 1.15],
    0x2660: [.719, .129, .894],
    0x2661: [.711, .024, .894],
    0x2662: [.719, .154, .894],
    0x2663: [.719, .129, .894],
    0x266D: [.75, .017, .447],
    0x266E: [.741, .223, .447],
    0x266F: [.724, .224, .447],
    0x2758: [.75, .249, .319],
    0x27E8: [.75, .249, .447],
    0x27E9: [.75, .249, .447],
    0x27F5: [.518, .017, 1.805],
    0x27F6: [.518, .017, 1.833],
    0x27F7: [.518, .017, 2.126],
    0x27F8: [.547, .046, 1.868],
    0x27F9: [.547, .046, 1.87],
    0x27FA: [.547, .046, 2.126],
    0x27FC: [.518, .017, 1.833],
    0x29F8: [.711, .21, .894],
    0x2A2F: [.53, .028, .894],
    0x2A3F: [.686, 0, .9],
    0x2AAF: [.696, .199, .894],
    0x2AB0: [.697, .199, .894],
    0x3008: [.75, .249, .447],
    0x3009: [.75, .249, .447],
};
