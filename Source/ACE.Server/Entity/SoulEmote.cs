using System.Collections.Generic;
using ACE.Entity.Enum;

namespace ACE.Server.Entity
{
    public class SoulEmote
    {
        public static HashSet<MotionCommand> SoulEmotes = new HashSet<MotionCommand>()
        {
            MotionCommand.AFKState,             // *afk*, *away from keyboard*, *away*
            MotionCommand.AkimboState,          // *akimbo*, *heroic*, *super*
            MotionCommand.AtEaseState,          // *at ease*, *atease*
            MotionCommand.ATOYOT,               // *atoyot*
            MotionCommand.Beckon,               // *beckon*, *beckons*, *come here*, *come*, *comehere*
            MotionCommand.BeSeeingYou,          // *bcingu*, *bcinu*, *be seeing you*, *beseeingyou*
            MotionCommand.BlowKiss,             // *blow kiss*, *blowkiss*, *kiss*, *kisses*
            MotionCommand.BowDeepState,         // *bow deep*, *bow*, *bowdeep*, *bows*
            MotionCommand.Cheer,                // *cheer*, *happy*, *joy*, *whoo hoo*, *woo hoo*, *woo-hoo*, *yay*, *yeppie*
            MotionCommand.ClapHands,            // *applause*, *clap hands*, *clap*, *claphands*, *claps*
            MotionCommand.ClapHandsState,       // *clapping hands*, *clapping*
            MotionCommand.Cringe,               // *cower*, *cringe*, *cringes*, *flinch*
            MotionCommand.CrossArmsState,       // *cross arms*, *crossarms*
            MotionCommand.Cry,                  // *cries*, *cry*, *sad*
            MotionCommand.CurtseyState,         // *curtsey*, *curtsy*
            MotionCommand.DrudgeDance,          // *dance step*, *dancestep*
            MotionCommand.DrudgeDanceState,     // *crazy dance*, *crazydance*, *dance crazy*, *dance like a drudge*, *dance*, *dancecrazy*, *drudge dance*, *drudgedance*
            MotionCommand.HaveASeat,            // *musical chair*, *musicalchair*
            MotionCommand.HaveASeatState,       // *have a seat*, *haveaseat*, *offer seat*, *offerseat*
            MotionCommand.HeartyLaugh,          // *big laugh*, *biglaugh*, *hearty laugh*, *heartylaugh*, *lol*
            MotionCommand.Helper,               // *available*, *helper*
            MotionCommand.KneelState,           // *kneel*, *kneels*
            MotionCommand.Knock,                // *knock*
            MotionCommand.Laugh,                // *ha ha*, *ha*, *hah hah*, *hah*, *haha*, *ha-ha*, *hah-hah*, *he he*, *heh heh*, *heh*, *hehe*, *he-he*, *hehehe*, *heh-heh*, *laugh*, *laughs*, *roflmao*
            MotionCommand.LeanState,            // *lean*
            MotionCommand.MeditateState,        // *meditate*, *pray kneel*, *praykneel*
            MotionCommand.MimeDrink,            // *drink*, *drinks*, *mime drink*, *mime drinking*, *mimedrink*, *mimedrinking*
            MotionCommand.MimeEat,              // *eat*, *eats*, *mime eat*, *mime eating*, *mimeeat*, *mimeeating*
            MotionCommand.Mock,                 // *mock*, *point and laugh*, *pointandlaugh*, *rofl*
            MotionCommand.Nod,                  // *k*, *nod*, *nods*, *ok*, *okay*, *yes*
            MotionCommand.NudgeLeft,            // *nudge left*, *nudgeleft*
            MotionCommand.NudgeRight,           // *nudge right*, *nudgeright*
            MotionCommand.PleadState,           // *grovel*, *grovels*, *plead*, *pleads*, *please*
            MotionCommand.PointDown,            // *point down*, *pointdown*
            MotionCommand.PointDownState,       // *point down state*, *pointdownstate*, *pointing down*, *pointingdown*, *points down*, *pointsdown*
            MotionCommand.PointLeft,            // *point left*, *pointleft*
            MotionCommand.PointLeftState,       // *gauche*, *point left state*, *pointing left*, *pointleftstate*, *points left*
            MotionCommand.PointRight,           // *point right*, *pointright*
            MotionCommand.PointRightState,      // *droit*, *point right state*, *pointing right*, *pointrightstate*, *points right*
            MotionCommand.PointState,           // *point*, *points*, *there*
            MotionCommand.PossumState,          // *opossum*, *play dead*, *play possum*, *playdead*, *playpossum*, *possum*
            MotionCommand.PrayState,            // *pray*
            MotionCommand.ReadState,            // *read a book*, *read something*, *read*, *readabook*, *readsomething*
            MotionCommand.SaluteState,          // *salute*, *salutes*, *yes sir*, *yessir*
            MotionCommand.ScanHorizon,          // *lookout*, *peer*, *scan horizon*, *scan*, *scanhorizon*
            MotionCommand.ScratchHead,          // *huh?*, *scratch head*, *scratches head*
            MotionCommand.ScratchHeadState,     // *hmm*, *hmmm*, *hmmmm*, *itchy*, *scratching head*, *scratching*, *scratchinghead*
            MotionCommand.ShakeFist,            // *angry*, *shake fist*, *shakefist*, *shakes fist*
            MotionCommand.ShakeFistState,       // *getting angry*, *shaking fist*, *shakingfist*
            MotionCommand.ShakeHead,            // *no*, *nope*, *shake head*, *shakes head*
            MotionCommand.Shiver,               // *brrr*, *cold*, *shiver*, *shivers*, *shudder*, *shudders*
            MotionCommand.Shoo,                 // *go away*, *goaway*, *shoo*, *shoos*
            MotionCommand.Shrug,                // *beats me*, *beatsme*, *dunno*, *got me*, *gotme*, *i don't know*, *i dunno*, *idontknow*, *idunno*, *shrug*, *shrugs*
            MotionCommand.SitBackState,         // *getreallycomfortable*, *sit back*, *sitback*, *sits back*, *sitting back*
            MotionCommand.SitCrossleggedState,  // *cross leg*, *cross legs*, *crossleg*, *crosslegs*, *getcomfortable*, *sit cross legged*, *sitcrosslegged*, *sits cross legged*, *sitting cross legged*, *sitting crosslegged*
            MotionCommand.SitState,             // *asseoir*, *sit down*, *sit*, *sitdown*, *sits down*, *sits*, *sitsdown*, *sitting*
            MotionCommand.SlouchState,          // *slouch*, *slouches*
            MotionCommand.SmackHead,            // *doh!*, *doh*, *oops*, *slap head*, *slaphead*, *smack head*, *smackhead*, *smacks head*, *smackshead*, *v8*
            MotionCommand.SnowAngelState,       // *snow angel*, *snowangel*
            MotionCommand.Spit,                 // *spit*, *spits*
            MotionCommand.SurrenderState,       // *give up*, *giveup*, *surrender*, *surrenders*
            MotionCommand.TalktotheHandState,   // *talk to hand*, *talk to the hand*, *talktohand*, *talktothehand*
            MotionCommand.TapFootState,         // *tap foot*, *tapfoot*, *tapping foot*, *tappingfoot*, *taps foot*, *wait*
            MotionCommand.Teapot,               // *i'm a little teapot*, *teapot*
            MotionCommand.ThinkerState,         // *think*, *thinker*
            MotionCommand.WarmHands,            // *blow hands*, *blow in hands*, *blow on hands*, *warm hands*, *warm up hands*
            MotionCommand.Wave,                 // *hello*, *hi*, *howdy*, *wave*, *wave1*, *waves*
            MotionCommand.WaveHigh,             // *wave high*, *wave2*, *wavehigh*, *waves high*, *waveshigh*
            MotionCommand.WaveLow,              // *wave low*, *wave3*, *wavelow*, *waves low*, *waveslow*
            MotionCommand.WaveState,            // *waving hand*, *waving*
            MotionCommand.WindedState,          // *winded*
            MotionCommand.WoahState,            // *stop*, *stops*, *whoa*, *woah*
            MotionCommand.YawnStretch,          // *stretch*, *stretches*, *tired*, *yawn*, *yawns*
            MotionCommand.YMCA,                 // *ymca*
        };
    }
}
