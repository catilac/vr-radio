using UnityEngine;
using System.Collections;

// all known gesture templates
public class GestureTemplates
{
    public static ArrayList Templates;
    public static ArrayList TemplateNames;

    public GestureTemplates ()
    {
        ArrayList line = new ArrayList(new Vector2[] {new Vector2(-250, 0), new Vector2(-242, 6), new Vector2(-234, 66), new Vector2(-227, 117), new Vector2(-219, 200), new Vector2(-211, 184), new Vector2(-203, 203), new Vector2(-195, 218), new Vector2(-187, 202), new Vector2(-179, 186), new Vector2(-171, 170), new Vector2(-163, 154), new Vector2(-155, 138), new Vector2(-147, 122), new Vector2(-139, 106), new Vector2(-131, 90), new Vector2(-123, 74), new Vector2(-115, 57), new Vector2(-107, 41), new Vector2(-99, 25), new Vector2(-91, 9), new Vector2(-84, -7), new Vector2(-76, -20), new Vector2(-68, -9), new Vector2(-60, 1), new Vector2(-52, 10), new Vector2(-44, 18), new Vector2(-36, 26), new Vector2(-28, 13), new Vector2(-20, -3), new Vector2(-12, -16), new Vector2(-4, -13), new Vector2(4, -10), new Vector2(12, -7), new Vector2(20, -1), new Vector2(28, 7), new Vector2(36, 15), new Vector2(44, 16), new Vector2(52, 18), new Vector2(60, 19), new Vector2(68, 18), new Vector2(76, 2), new Vector2(84, -14), new Vector2(91, -30), new Vector2(99, -40), new Vector2(107, -31), new Vector2(115, -21), new Vector2(123, -28), new Vector2(131, -44), new Vector2(139, -60), new Vector2(147, -76), new Vector2(155, -92), new Vector2(163, -108), new Vector2(171, -124), new Vector2(179, -140), new Vector2(187, -156), new Vector2(195, -172), new Vector2(203, -175), new Vector2(211, -142), new Vector2(219, -154), new Vector2(227, -170), new Vector2(235, -186), new Vector2(243, -202), new Vector2(250, -282)});

        ArrayList line2 = new ArrayList(new Vector2[] {new Vector2(-250, 0), new Vector2(-242, -11), new Vector2(-234, -23), new Vector2(-227, -34), new Vector2(-219, -46), new Vector2(-211, -57), new Vector2(-203, -69), new Vector2(-195, -80), new Vector2(-187, -91), new Vector2(-179, -103), new Vector2(-171, -114), new Vector2(-163, -126), new Vector2(-155, -137), new Vector2(-147, -149), new Vector2(-139, -160), new Vector2(-131, -159), new Vector2(-123, -154), new Vector2(-115, -150), new Vector2(-107, -145), new Vector2(-99, -135), new Vector2(-91, -123), new Vector2(-83, -111), new Vector2(-75, -85), new Vector2(-68, -57), new Vector2(-60, -29), new Vector2(-52, -21), new Vector2(-44, -32), new Vector2(-36, -44), new Vector2(-28, -55), new Vector2(-20, -67), new Vector2(-12, -78), new Vector2(-4, -90), new Vector2(4, -101), new Vector2(12, -112), new Vector2(20, -121), new Vector2(28, -115), new Vector2(36, -109), new Vector2(44, -102), new Vector2(52, -85), new Vector2(60, -52), new Vector2(68, -20), new Vector2(76, 12), new Vector2(83, 44), new Vector2(91, 60), new Vector2(99, 74), new Vector2(107, 90), new Vector2(115, 112), new Vector2(123, 135), new Vector2(131, 157), new Vector2(139, 168), new Vector2(147, 157), new Vector2(155, 156), new Vector2(163, 170), new Vector2(171, 184), new Vector2(179, 177), new Vector2(187, 166), new Vector2(195, 154), new Vector2(203, 143), new Vector2(211, 147), new Vector2(219, 178), new Vector2(227, 208), new Vector2(234, 240), new Vector2(242, 284), new Vector2(250, 340)});

        // add all templates
        Templates = new ArrayList(new ArrayList[] {line, line2});
        TemplateNames = new ArrayList(new string[] {"line", "line2"});
    }
}