using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFileBuilder
{
    class IrregularVerbs
    {

        public static List<Tuple<string, string>> GetIrregularVerbsFormsAndAssociatedLemma()
        {
            var results = new List<Tuple<string, string>>();
            foreach (var iv in AllIrregularVerbs)
            {
                var allDerivedForms = iv.PastParticiple
                    .Union(iv.Preterit)
                    .Union(new List<string>() {iv.PresentParticiple, iv.ThirdPersonSingular})
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Distinct()
                    .Select(s => new Tuple<string, string>(s.ToLowerInvariant(), iv.BaseForm.ToLowerInvariant()))
                    .ToList();
                results.AddRange(allDerivedForms);
            }

            return results;
        }

        // List of irregular verbs retrieved on http://www.englishpage.com/irregularverbs/irregularverbs.html
        private static readonly List<IrregularVerb> AllIrregularVerbs = new List<IrregularVerb>()
        {
            new IrregularVerb()
            {
                BaseForm = "Abide",
                ThirdPersonSingular = "Abides",
                PresentParticiple = "Abiding",
                Preterit = new List<string>()
                {
                    "Abode",
                    "Abided",
                },
                PastParticiple = new List<string>()
                {
                    "Abode",
                    "Abided",
                    "Abidden",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Alight",
                ThirdPersonSingular = "Alights",
                PresentParticiple = "Alighting",
                Preterit = new List<string>()
                {
                    "Alit",
                    "Alighted",
                },
                PastParticiple = new List<string>()
                {
                    "Alit",
                    "Alighted",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Arise",
                ThirdPersonSingular = "Arises",
                PresentParticiple = "Arising",
                Preterit = new List<string>()
                {
                    "Arose",
                },
                PastParticiple = new List<string>()
                {
                    "Arisen",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Awake",
                ThirdPersonSingular = "Awakes",
                PresentParticiple = "Awaking",
                Preterit = new List<string>()
                {
                    "Awoke",
                },
                PastParticiple = new List<string>()
                {
                    "Awoken",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Be",
                ThirdPersonSingular = "Is",
                PresentParticiple = "Being",
                Preterit = new List<string>()
                {
                    "Was",
                    "Were",
                },
                PastParticiple = new List<string>()
                {
                    "Been",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Bear",
                ThirdPersonSingular = "Bears",
                PresentParticiple = "Bearing",
                Preterit = new List<string>()
                {
                    "Bore",
                },
                PastParticiple = new List<string>()
                {
                    "Born",
                    "Borne",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Beat",
                ThirdPersonSingular = "Beats",
                PresentParticiple = "Beating",
                Preterit = new List<string>()
                {
                    "Beat",
                },
                PastParticiple = new List<string>()
                {
                    "Beaten",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Become",
                ThirdPersonSingular = "Becomes",
                PresentParticiple = "Becoming",
                Preterit = new List<string>()
                {
                    "Became",
                },
                PastParticiple = new List<string>()
                {
                    "Become",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Begin",
                ThirdPersonSingular = "Begins",
                PresentParticiple = "Beginning",
                Preterit = new List<string>()
                {
                    "Began",
                },
                PastParticiple = new List<string>()
                {
                    "Begun",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Behold",
                ThirdPersonSingular = "Beholds",
                PresentParticiple = "Beholding",
                Preterit = new List<string>()
                {
                    "Beheld",
                },
                PastParticiple = new List<string>()
                {
                    "Beheld",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Bend",
                ThirdPersonSingular = "Bends",
                PresentParticiple = "Bending",
                Preterit = new List<string>()
                {
                    "Bent",
                },
                PastParticiple = new List<string>()
                {
                    "Bent",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Bet",
                ThirdPersonSingular = "Bets",
                PresentParticiple = "Betting",
                Preterit = new List<string>()
                {
                    "Bet",
                },
                PastParticiple = new List<string>()
                {
                    "Bet",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Bid",
                ThirdPersonSingular = "Bids",
                PresentParticiple = "Bidding",
                Preterit = new List<string>()
                {
                    "Bade",
                },
                PastParticiple = new List<string>()
                {
                    "Bidden",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Bid",
                ThirdPersonSingular = "Bids",
                PresentParticiple = "Bidding",
                Preterit = new List<string>()
                {
                    "Bid",
                },
                PastParticiple = new List<string>()
                {
                    "Bid",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Bind",
                ThirdPersonSingular = "Binds",
                PresentParticiple = "Binding",
                Preterit = new List<string>()
                {
                    "Bound",
                },
                PastParticiple = new List<string>()
                {
                    "Bound",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Bite",
                ThirdPersonSingular = "Bites",
                PresentParticiple = "Biting",
                Preterit = new List<string>()
                {
                    "Bit",
                },
                PastParticiple = new List<string>()
                {
                    "Bitten",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Bleed",
                ThirdPersonSingular = "Bleeds",
                PresentParticiple = "Bleeding",
                Preterit = new List<string>()
                {
                    "Bled",
                },
                PastParticiple = new List<string>()
                {
                    "Bled",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Blow",
                ThirdPersonSingular = "Blows",
                PresentParticiple = "Blowing",
                Preterit = new List<string>()
                {
                    "Blew",
                },
                PastParticiple = new List<string>()
                {
                    "Blown",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Break",
                ThirdPersonSingular = "Breaks",
                PresentParticiple = "Breaking",
                Preterit = new List<string>()
                {
                    "Broke",
                },
                PastParticiple = new List<string>()
                {
                    "Broken",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Breed",
                ThirdPersonSingular = "Breeds",
                PresentParticiple = "Breeding",
                Preterit = new List<string>()
                {
                    "Bred",
                },
                PastParticiple = new List<string>()
                {
                    "Bred",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Bring",
                ThirdPersonSingular = "Brings",
                PresentParticiple = "Bringing",
                Preterit = new List<string>()
                {
                    "Brought",
                },
                PastParticiple = new List<string>()
                {
                    "Brought",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Broadcast",
                ThirdPersonSingular = "Broadcasts",
                PresentParticiple = "Broadcasting",
                Preterit = new List<string>()
                {
                    "Broadcast",
                    "Broadcasted",
                },
                PastParticiple = new List<string>()
                {
                    "Broadcast",
                    "Broadcasted",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Build",
                ThirdPersonSingular = "Builds",
                PresentParticiple = "Building",
                Preterit = new List<string>()
                {
                    "Built",
                },
                PastParticiple = new List<string>()
                {
                    "Built",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Burn",
                ThirdPersonSingular = "Burns",
                PresentParticiple = "Burning",
                Preterit = new List<string>()
                {
                    "Burnt",
                    "Burned",
                },
                PastParticiple = new List<string>()
                {
                    "Burnt",
                    "Burned",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Burst",
                ThirdPersonSingular = "Bursts",
                PresentParticiple = "Bursting",
                Preterit = new List<string>()
                {
                    "Burst",
                },
                PastParticiple = new List<string>()
                {
                    "Burst",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Bust",
                ThirdPersonSingular = "Busts",
                PresentParticiple = "Busting",
                Preterit = new List<string>()
                {
                    "Bust",
                },
                PastParticiple = new List<string>()
                {
                    "Bust",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Buy",
                ThirdPersonSingular = "Buys",
                PresentParticiple = "Buying",
                Preterit = new List<string>()
                {
                    "Bought",
                },
                PastParticiple = new List<string>()
                {
                    "Bought",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Cast",
                ThirdPersonSingular = "Casts",
                PresentParticiple = "Casting",
                Preterit = new List<string>()
                {
                    "Cast",
                },
                PastParticiple = new List<string>()
                {
                    "Cast",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Catch",
                ThirdPersonSingular = "Catches",
                PresentParticiple = "Catching",
                Preterit = new List<string>()
                {
                    "Caught",
                },
                PastParticiple = new List<string>()
                {
                    "Caught",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Choose",
                ThirdPersonSingular = "Chooses",
                PresentParticiple = "Choosing",
                Preterit = new List<string>()
                {
                    "Chose",
                },
                PastParticiple = new List<string>()
                {
                    "Chosen",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Clap",
                ThirdPersonSingular = "Claps",
                PresentParticiple = "Clapping",
                Preterit = new List<string>()
                {
                    "Clapped",
                    "Clapt",
                },
                PastParticiple = new List<string>()
                {
                    "Clapped",
                    "Clapt",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Cling",
                ThirdPersonSingular = "Clings",
                PresentParticiple = "Clinging",
                Preterit = new List<string>()
                {
                    "Clung",
                },
                PastParticiple = new List<string>()
                {
                    "Clung",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Clothe",
                ThirdPersonSingular = "Clothes",
                PresentParticiple = "Clothing",
                Preterit = new List<string>()
                {
                    "Clad",
                    "Clothed",
                },
                PastParticiple = new List<string>()
                {
                    "Clad",
                    "Clothed",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Come",
                ThirdPersonSingular = "Comes",
                PresentParticiple = "Coming",
                Preterit = new List<string>()
                {
                    "Came",
                },
                PastParticiple = new List<string>()
                {
                    "Come",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Cost",
                ThirdPersonSingular = "Costs",
                PresentParticiple = "Costing",
                Preterit = new List<string>()
                {
                    "Cost",
                },
                PastParticiple = new List<string>()
                {
                    "Cost",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Creep",
                ThirdPersonSingular = "Creeps",
                PresentParticiple = "Creeping",
                Preterit = new List<string>()
                {
                    "Crept",
                },
                PastParticiple = new List<string>()
                {
                    "Crept",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Cut",
                ThirdPersonSingular = "Cuts",
                PresentParticiple = "Cutting",
                Preterit = new List<string>()
                {
                    "Cut",
                },
                PastParticiple = new List<string>()
                {
                    "Cut",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Dare",
                ThirdPersonSingular = "Dares",
                PresentParticiple = "Daring",
                Preterit = new List<string>()
                {
                    "Dared",
                    "Durst",
                },
                PastParticiple = new List<string>()
                {
                    "Dared",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Deal",
                ThirdPersonSingular = "Deals",
                PresentParticiple = "Dealing",
                Preterit = new List<string>()
                {
                    "Dealt",
                },
                PastParticiple = new List<string>()
                {
                    "Dealt",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Dig",
                ThirdPersonSingular = "Digs",
                PresentParticiple = "Digging",
                Preterit = new List<string>()
                {
                    "Dug",
                },
                PastParticiple = new List<string>()
                {
                    "Dug",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Dive",
                ThirdPersonSingular = "Dives",
                PresentParticiple = "Diving",
                Preterit = new List<string>()
                {
                    "Dived",
                    "Dove",
                },
                PastParticiple = new List<string>()
                {
                    "Dived",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Do",
                ThirdPersonSingular = "Does",
                PresentParticiple = "Doing",
                Preterit = new List<string>()
                {
                    "Did",
                },
                PastParticiple = new List<string>()
                {
                    "Done",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Draw",
                ThirdPersonSingular = "Draws",
                PresentParticiple = "Drawing",
                Preterit = new List<string>()
                {
                    "Drew",
                },
                PastParticiple = new List<string>()
                {
                    "Drawn",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Dream",
                ThirdPersonSingular = "Dreams",
                PresentParticiple = "Dreaming",
                Preterit = new List<string>()
                {
                    "Dreamt",
                    "Dreamed",
                },
                PastParticiple = new List<string>()
                {
                    "Dreamt",
                    "Dreamed",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Drink",
                ThirdPersonSingular = "Drinks",
                PresentParticiple = "Drinking",
                Preterit = new List<string>()
                {
                    "Drank",
                },
                PastParticiple = new List<string>()
                {
                    "Drunk",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Drive",
                ThirdPersonSingular = "Drives",
                PresentParticiple = "Driving",
                Preterit = new List<string>()
                {
                    "Drove",
                },
                PastParticiple = new List<string>()
                {
                    "Driven",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Dwell",
                ThirdPersonSingular = "Dwells",
                PresentParticiple = "Dwelling",
                Preterit = new List<string>()
                {
                    "Dwelt",
                },
                PastParticiple = new List<string>()
                {
                    "Dwelt",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Eat",
                ThirdPersonSingular = "Eats",
                PresentParticiple = "Eating",
                Preterit = new List<string>()
                {
                    "Ate",
                },
                PastParticiple = new List<string>()
                {
                    "Eaten",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Fall",
                ThirdPersonSingular = "Falls",
                PresentParticiple = "Falling",
                Preterit = new List<string>()
                {
                    "Fell",
                },
                PastParticiple = new List<string>()
                {
                    "Fallen",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Feed",
                ThirdPersonSingular = "Feeds",
                PresentParticiple = "Feeding",
                Preterit = new List<string>()
                {
                    "Fed",
                },
                PastParticiple = new List<string>()
                {
                    "Fed",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Feel",
                ThirdPersonSingular = "Feels",
                PresentParticiple = "Feeling",
                Preterit = new List<string>()
                {
                    "Felt",
                },
                PastParticiple = new List<string>()
                {
                    "Felt",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Fight",
                ThirdPersonSingular = "Fights",
                PresentParticiple = "Fighting",
                Preterit = new List<string>()
                {
                    "Fought",
                },
                PastParticiple = new List<string>()
                {
                    "Fought",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Find",
                ThirdPersonSingular = "Finds",
                PresentParticiple = "Finding",
                Preterit = new List<string>()
                {
                    "Found",
                },
                PastParticiple = new List<string>()
                {
                    "Found",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Fit",
                ThirdPersonSingular = "Fits",
                PresentParticiple = "Fitting",
                Preterit = new List<string>()
                {
                    "Fit",
                    "Fitted",
                },
                PastParticiple = new List<string>()
                {
                    "Fit",
                    "Fitted",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Flee",
                ThirdPersonSingular = "Flees",
                PresentParticiple = "Fleeing",
                Preterit = new List<string>()
                {
                    "Fled",
                },
                PastParticiple = new List<string>()
                {
                    "Fled",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Fling",
                ThirdPersonSingular = "Flings",
                PresentParticiple = "Flinging",
                Preterit = new List<string>()
                {
                    "Flung",
                },
                PastParticiple = new List<string>()
                {
                    "Flung",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Fly",
                ThirdPersonSingular = "Flies",
                PresentParticiple = "Flying",
                Preterit = new List<string>()
                {
                    "Flew",
                },
                PastParticiple = new List<string>()
                {
                    "Flown",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Forbid",
                ThirdPersonSingular = "Forbids",
                PresentParticiple = "Forbidding",
                Preterit = new List<string>()
                {
                    "Forbade",
                    "Forbad",
                },
                PastParticiple = new List<string>()
                {
                    "Forbidden",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Forecast",
                ThirdPersonSingular = "Forecasts",
                PresentParticiple = "Forecasting",
                Preterit = new List<string>()
                {
                    "Forecast",
                    "Forecasted",
                },
                PastParticiple = new List<string>()
                {
                    "Forecast",
                    "Forecasted",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Foresee",
                ThirdPersonSingular = "Foresees",
                PresentParticiple = "Foreseeing",
                Preterit = new List<string>()
                {
                    "Foresaw",
                },
                PastParticiple = new List<string>()
                {
                    "Foreseen",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Foretell",
                ThirdPersonSingular = "Foretells",
                PresentParticiple = "Foretelling",
                Preterit = new List<string>()
                {
                    "Foretold",
                },
                PastParticiple = new List<string>()
                {
                    "Foretold",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Forget",
                ThirdPersonSingular = "Forgets",
                PresentParticiple = "Foregetting",
                Preterit = new List<string>()
                {
                    "Forgot",
                },
                PastParticiple = new List<string>()
                {
                    "Forgotten",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Forgive",
                ThirdPersonSingular = "Forgives",
                PresentParticiple = "Forgiving",
                Preterit = new List<string>()
                {
                    "Forgave",
                },
                PastParticiple = new List<string>()
                {
                    "Forgiven",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Forsake",
                ThirdPersonSingular = "Forsakes",
                PresentParticiple = "Forsaking",
                Preterit = new List<string>()
                {
                    "Forsook",
                },
                PastParticiple = new List<string>()
                {
                    "Forsaken",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Freeze",
                ThirdPersonSingular = "Freezes",
                PresentParticiple = "Freezing",
                Preterit = new List<string>()
                {
                    "Froze",
                },
                PastParticiple = new List<string>()
                {
                    "Frozen",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Frostbite",
                ThirdPersonSingular = "Frostbites",
                PresentParticiple = "Frostbiting",
                Preterit = new List<string>()
                {
                    "Frostbit",
                },
                PastParticiple = new List<string>()
                {
                    "Frostbitten",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Get",
                ThirdPersonSingular = "Gets",
                PresentParticiple = "Getting",
                Preterit = new List<string>()
                {
                    "Got",
                },
                PastParticiple = new List<string>()
                {
                    "Got",
                    "Gotten",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Give",
                ThirdPersonSingular = "Gives",
                PresentParticiple = "Giving",
                Preterit = new List<string>()
                {
                    "Gave",
                },
                PastParticiple = new List<string>()
                {
                    "Given",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Go",
                ThirdPersonSingular = "Goes",
                PresentParticiple = "Going",
                Preterit = new List<string>()
                {
                    "Went",
                },
                PastParticiple = new List<string>()
                {
                    "Gone",
                    "Been",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Grind",
                ThirdPersonSingular = "Grinds",
                PresentParticiple = "Grinding",
                Preterit = new List<string>()
                {
                    "Ground",
                },
                PastParticiple = new List<string>()
                {
                    "Ground",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Grow",
                ThirdPersonSingular = "Grows",
                PresentParticiple = "Growing",
                Preterit = new List<string>()
                {
                    "Grew",
                },
                PastParticiple = new List<string>()
                {
                    "Grown",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Handwrite",
                ThirdPersonSingular = "Handwrites",
                PresentParticiple = "Handwriting",
                Preterit = new List<string>()
                {
                    "Handwrote",
                },
                PastParticiple = new List<string>()
                {
                    "Handwritten",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Hang",
                ThirdPersonSingular = "Hangs",
                PresentParticiple = "Hanging",
                Preterit = new List<string>()
                {
                    "Hung",
                    "Hanged",
                },
                PastParticiple = new List<string>()
                {
                    "Hung",
                    "Hanged",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Have",
                ThirdPersonSingular = "Has",
                PresentParticiple = "Having",
                Preterit = new List<string>()
                {
                    "Had",
                },
                PastParticiple = new List<string>()
                {
                    "Had",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Hear",
                ThirdPersonSingular = "Hears",
                PresentParticiple = "Hearing",
                Preterit = new List<string>()
                {
                    "Heard",
                },
                PastParticiple = new List<string>()
                {
                    "Heard",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Hide",
                ThirdPersonSingular = "Hides",
                PresentParticiple = "Hiding",
                Preterit = new List<string>()
                {
                    "Hid",
                },
                PastParticiple = new List<string>()
                {
                    "Hidden",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Hit",
                ThirdPersonSingular = "Hits",
                PresentParticiple = "Hitting",
                Preterit = new List<string>()
                {
                    "Hit",
                },
                PastParticiple = new List<string>()
                {
                    "Hit",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Hold",
                ThirdPersonSingular = "Holds",
                PresentParticiple = "Holding",
                Preterit = new List<string>()
                {
                    "Held",
                },
                PastParticiple = new List<string>()
                {
                    "Held",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Hurt",
                ThirdPersonSingular = "Hurts",
                PresentParticiple = "Hurting",
                Preterit = new List<string>()
                {
                    "Hurt",
                },
                PastParticiple = new List<string>()
                {
                    "Hurt",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Inlay",
                ThirdPersonSingular = "Inlays",
                PresentParticiple = "Inlaying",
                Preterit = new List<string>()
                {
                    "Inlaid",
                },
                PastParticiple = new List<string>()
                {
                    "Inlaid",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Input",
                ThirdPersonSingular = "Inputs",
                PresentParticiple = "Inputting",
                Preterit = new List<string>()
                {
                    "Input",
                    "Inputted",
                },
                PastParticiple = new List<string>()
                {
                    "Input",
                    "Inputted",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Interlay",
                ThirdPersonSingular = "Interlays",
                PresentParticiple = "Interlaying",
                Preterit = new List<string>()
                {
                    "Interlaid",
                },
                PastParticiple = new List<string>()
                {
                    "Interlaid",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Keep",
                ThirdPersonSingular = "Keeps",
                PresentParticiple = "Keeping",
                Preterit = new List<string>()
                {
                    "Kept",
                },
                PastParticiple = new List<string>()
                {
                    "Kept",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Kneel",
                ThirdPersonSingular = "Kneels",
                PresentParticiple = "Kneeling",
                Preterit = new List<string>()
                {
                    "Knelt",
                    "Kneeled",
                },
                PastParticiple = new List<string>()
                {
                    "Knelt",
                    "Kneeled",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Knit",
                ThirdPersonSingular = "Knits",
                PresentParticiple = "Knitting",
                Preterit = new List<string>()
                {
                    "Knit",
                    "Knitted",
                },
                PastParticiple = new List<string>()
                {
                    "Knit",
                    "Knitted",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Know",
                ThirdPersonSingular = "Knows",
                PresentParticiple = "Knowing",
                Preterit = new List<string>()
                {
                    "Knew",
                },
                PastParticiple = new List<string>()
                {
                    "Known",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Lay",
                ThirdPersonSingular = "Lays",
                PresentParticiple = "laying",
                Preterit = new List<string>()
                {
                    "Laid",
                },
                PastParticiple = new List<string>()
                {
                    "Laid",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Lead",
                ThirdPersonSingular = "Leads",
                PresentParticiple = "Leading",
                Preterit = new List<string>()
                {
                    "Led",
                },
                PastParticiple = new List<string>()
                {
                    "Led",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Lean",
                ThirdPersonSingular = "Leans",
                PresentParticiple = "Leaning",
                Preterit = new List<string>()
                {
                    "Leant",
                    "Leaned",
                },
                PastParticiple = new List<string>()
                {
                    "Leant",
                    "Leaned",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Leap",
                ThirdPersonSingular = "Leaps",
                PresentParticiple = "Leaping",
                Preterit = new List<string>()
                {
                    "Leapt",
                    "Leaped",
                },
                PastParticiple = new List<string>()
                {
                    "Leapt",
                    "Leaped",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Learn",
                ThirdPersonSingular = "Learns",
                PresentParticiple = "Learning",
                Preterit = new List<string>()
                {
                    "Learnt",
                    "Learned",
                },
                PastParticiple = new List<string>()
                {
                    "Learnt",
                    "Learned",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Leave",
                ThirdPersonSingular = "Leaves",
                PresentParticiple = "Leaving",
                Preterit = new List<string>()
                {
                    "Left",
                },
                PastParticiple = new List<string>()
                {
                    "Left",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Lend",
                ThirdPersonSingular = "Lends",
                PresentParticiple = "Lending",
                Preterit = new List<string>()
                {
                    "Lent",
                },
                PastParticiple = new List<string>()
                {
                    "Lent",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Let",
                ThirdPersonSingular = "Lets",
                PresentParticiple = "Letting",
                Preterit = new List<string>()
                {
                    "Let",
                },
                PastParticiple = new List<string>()
                {
                    "Let",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Lie",
                ThirdPersonSingular = "Lies",
                PresentParticiple = "Lying",
                Preterit = new List<string>()
                {
                    "Lay",
                },
                PastParticiple = new List<string>()
                {
                    "Lain",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Light",
                ThirdPersonSingular = "Lights",
                PresentParticiple = "Lighting",
                Preterit = new List<string>()
                {
                    "Lit",
                },
                PastParticiple = new List<string>()
                {
                    "Lit",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Lose",
                ThirdPersonSingular = "Loses",
                PresentParticiple = "Losing",
                Preterit = new List<string>()
                {
                    "Lost",
                },
                PastParticiple = new List<string>()
                {
                    "Lost",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Make",
                ThirdPersonSingular = "Makes",
                PresentParticiple = "Making",
                Preterit = new List<string>()
                {
                    "Made",
                },
                PastParticiple = new List<string>()
                {
                    "Made",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Mean",
                ThirdPersonSingular = "Means",
                PresentParticiple = "Meaning",
                Preterit = new List<string>()
                {
                    "Meant",
                },
                PastParticiple = new List<string>()
                {
                    "Meant",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Meet",
                ThirdPersonSingular = "Meets",
                PresentParticiple = "Meeting",
                Preterit = new List<string>()
                {
                    "Met",
                },
                PastParticiple = new List<string>()
                {
                    "Met",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Melt",
                ThirdPersonSingular = "Melts",
                PresentParticiple = "Melting",
                Preterit = new List<string>()
                {
                    "Melted",
                },
                PastParticiple = new List<string>()
                {
                    "Molten",
                    "Melted",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Mislead",
                ThirdPersonSingular = "Misleads",
                PresentParticiple = "Misleading",
                Preterit = new List<string>()
                {
                    "Misled",
                },
                PastParticiple = new List<string>()
                {
                    "Misled",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Mistake",
                ThirdPersonSingular = "Mistakes",
                PresentParticiple = "Mistaking",
                Preterit = new List<string>()
                {
                    "Mistook",
                },
                PastParticiple = new List<string>()
                {
                    "Mistaken",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Misunderstand",
                ThirdPersonSingular = "Misunderstands",
                PresentParticiple = "Misunderstanding",
                Preterit = new List<string>()
                {
                    "Misunderstood",
                },
                PastParticiple = new List<string>()
                {
                    "Misunderstood",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Miswed",
                ThirdPersonSingular = "Misweds",
                PresentParticiple = "Miswedding",
                Preterit = new List<string>()
                {
                    "Miswed",
                    "Miswedded",
                },
                PastParticiple = new List<string>()
                {
                    "Miswed",
                    "Miswedded",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Mow",
                ThirdPersonSingular = "Mows",
                PresentParticiple = "Mowing",
                Preterit = new List<string>()
                {
                    "Mowed",
                },
                PastParticiple = new List<string>()
                {
                    "Mown",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Overdraw",
                ThirdPersonSingular = "Overdraws",
                PresentParticiple = "Overdrawing",
                Preterit = new List<string>()
                {
                    "Overdrew",
                },
                PastParticiple = new List<string>()
                {
                    "Overdrawn",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Overhear",
                ThirdPersonSingular = "Overhears",
                PresentParticiple = "Overhearing",
                Preterit = new List<string>()
                {
                    "Overheard",
                },
                PastParticiple = new List<string>()
                {
                    "Overheard",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Overtake",
                ThirdPersonSingular = "Overtakes",
                PresentParticiple = "Overtaking",
                Preterit = new List<string>()
                {
                    "Overtook",
                },
                PastParticiple = new List<string>()
                {
                    "Overtaken",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Pay",
                ThirdPersonSingular = "Pays",
                PresentParticiple = "Paying",
                Preterit = new List<string>()
                {
                    "Paid",
                },
                PastParticiple = new List<string>()
                {
                    "Paid",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Preset",
                ThirdPersonSingular = "Presets",
                PresentParticiple = "Presetting",
                Preterit = new List<string>()
                {
                    "Preset",
                },
                PastParticiple = new List<string>()
                {
                    "Preset",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Prove",
                ThirdPersonSingular = "Proves",
                PresentParticiple = "Proving",
                Preterit = new List<string>()
                {
                    "Proved",
                },
                PastParticiple = new List<string>()
                {
                    "Proven",
                    "Proved",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Put",
                ThirdPersonSingular = "Puts",
                PresentParticiple = "Putting",
                Preterit = new List<string>()
                {
                    "Put",
                },
                PastParticiple = new List<string>()
                {
                    "Put",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Quit",
                ThirdPersonSingular = "Quits",
                PresentParticiple = "Quitting",
                Preterit = new List<string>()
                {
                    "Quit",
                },
                PastParticiple = new List<string>()
                {
                    "Quit",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Re",
                ThirdPersonSingular = "Re",
                PresentParticiple = "Re",
                Preterit = new List<string>()
                {
                    "Re-proved",
                },
                PastParticiple = new List<string>()
                {
                    "Re-proven",
                    "Re-proved",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Read",
                ThirdPersonSingular = "Reads",
                PresentParticiple = "Reading",
                Preterit = new List<string>()
                {
                    "Read",
                },
                PastParticiple = new List<string>()
                {
                    "Read",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Rid",
                ThirdPersonSingular = "Rids",
                PresentParticiple = "Ridding",
                Preterit = new List<string>()
                {
                    "Rid",
                    "Ridded",
                },
                PastParticiple = new List<string>()
                {
                    "Rid",
                    "Ridded",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Ride",
                ThirdPersonSingular = "Rides",
                PresentParticiple = "Riding",
                Preterit = new List<string>()
                {
                    "Rode",
                },
                PastParticiple = new List<string>()
                {
                    "Ridden",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Ring",
                ThirdPersonSingular = "Rings",
                PresentParticiple = "Ringing",
                Preterit = new List<string>()
                {
                    "Rang",
                },
                PastParticiple = new List<string>()
                {
                    "Rung",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Rise",
                ThirdPersonSingular = "Rises",
                PresentParticiple = "Rising",
                Preterit = new List<string>()
                {
                    "Rose",
                },
                PastParticiple = new List<string>()
                {
                    "Risen",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Rive",
                ThirdPersonSingular = "Rives",
                PresentParticiple = "Riving",
                Preterit = new List<string>()
                {
                    "Rived",
                },
                PastParticiple = new List<string>()
                {
                    "Riven",
                    "Rived",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Run",
                ThirdPersonSingular = "Runs",
                PresentParticiple = "Running",
                Preterit = new List<string>()
                {
                    "Ran",
                },
                PastParticiple = new List<string>()
                {
                    "Run",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Saw",
                ThirdPersonSingular = "Saws",
                PresentParticiple = "Sawing",
                Preterit = new List<string>()
                {
                    "Sawed",
                },
                PastParticiple = new List<string>()
                {
                    "Sawn",
                    "Sawed",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Say",
                ThirdPersonSingular = "Says",
                PresentParticiple = "Saying",
                Preterit = new List<string>()
                {
                    "Said",
                },
                PastParticiple = new List<string>()
                {
                    "Said",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "See",
                ThirdPersonSingular = "Sees",
                PresentParticiple = "Seeing",
                Preterit = new List<string>()
                {
                    "Saw",
                },
                PastParticiple = new List<string>()
                {
                    "Seen",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Seek",
                ThirdPersonSingular = "Seeks",
                PresentParticiple = "Seeking",
                Preterit = new List<string>()
                {
                    "Sought",
                },
                PastParticiple = new List<string>()
                {
                    "Sought",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sell",
                ThirdPersonSingular = "Sells",
                PresentParticiple = "Selling",
                Preterit = new List<string>()
                {
                    "Sold",
                },
                PastParticiple = new List<string>()
                {
                    "Sold",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Send",
                ThirdPersonSingular = "Sends",
                PresentParticiple = "Sending",
                Preterit = new List<string>()
                {
                    "Sent",
                },
                PastParticiple = new List<string>()
                {
                    "Sent",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Set",
                ThirdPersonSingular = "Sets",
                PresentParticiple = "Setting",
                Preterit = new List<string>()
                {
                    "Set",
                },
                PastParticiple = new List<string>()
                {
                    "Set",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sew",
                ThirdPersonSingular = "Sews",
                PresentParticiple = "Sewing",
                Preterit = new List<string>()
                {
                    "Sewed",
                },
                PastParticiple = new List<string>()
                {
                    "Sewn",
                    "Sewed",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Shake",
                ThirdPersonSingular = "Shakes",
                PresentParticiple = "Shaking",
                Preterit = new List<string>()
                {
                    "Shook",
                },
                PastParticiple = new List<string>()
                {
                    "Shaken",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Shave",
                ThirdPersonSingular = "Shaves",
                PresentParticiple = "Shaving",
                Preterit = new List<string>()
                {
                    "Shaved",
                },
                PastParticiple = new List<string>()
                {
                    "Shaven",
                    "Shaved",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Shear",
                ThirdPersonSingular = "Shears",
                PresentParticiple = "Shearing",
                Preterit = new List<string>()
                {
                    "Shore",
                    "Sheared",
                },
                PastParticiple = new List<string>()
                {
                    "Shorn",
                    "Sheared",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Shed",
                ThirdPersonSingular = "Sheds",
                PresentParticiple = "Shedding",
                Preterit = new List<string>()
                {
                    "Shed",
                },
                PastParticiple = new List<string>()
                {
                    "Shed",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Shine",
                ThirdPersonSingular = "Shines",
                PresentParticiple = "Shining",
                Preterit = new List<string>()
                {
                    "Shone",
                },
                PastParticiple = new List<string>()
                {
                    "Shone",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Shoe",
                ThirdPersonSingular = "Shoes",
                PresentParticiple = "Shoeing",
                Preterit = new List<string>()
                {
                    "Shod",
                },
                PastParticiple = new List<string>()
                {
                    "Shod",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Shoot",
                ThirdPersonSingular = "Shoots",
                PresentParticiple = "Shooting",
                Preterit = new List<string>()
                {
                    "Shot",
                },
                PastParticiple = new List<string>()
                {
                    "Shot",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Show",
                ThirdPersonSingular = "Shows",
                PresentParticiple = "Showing",
                Preterit = new List<string>()
                {
                    "Showed",
                },
                PastParticiple = new List<string>()
                {
                    "Shown",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Shrink",
                ThirdPersonSingular = "Shrinks",
                PresentParticiple = "Shrinking",
                Preterit = new List<string>()
                {
                    "Shrank",
                },
                PastParticiple = new List<string>()
                {
                    "Shrunk",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Shut",
                ThirdPersonSingular = "Shuts",
                PresentParticiple = "Shutting",
                Preterit = new List<string>()
                {
                    "Shut",
                },
                PastParticiple = new List<string>()
                {
                    "Shut",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sing",
                ThirdPersonSingular = "Sings",
                PresentParticiple = "Singing",
                Preterit = new List<string>()
                {
                    "Sang",
                },
                PastParticiple = new List<string>()
                {
                    "Sung",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sink",
                ThirdPersonSingular = "Sinks",
                PresentParticiple = "Sinking",
                Preterit = new List<string>()
                {
                    "Sank",
                },
                PastParticiple = new List<string>()
                {
                    "Sunk",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sit",
                ThirdPersonSingular = "Sits",
                PresentParticiple = "Sitting",
                Preterit = new List<string>()
                {
                    "Sat",
                },
                PastParticiple = new List<string>()
                {
                    "Sat",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Slay",
                ThirdPersonSingular = "Slays",
                PresentParticiple = "Slaying",
                Preterit = new List<string>()
                {
                    "Slew",
                },
                PastParticiple = new List<string>()
                {
                    "Slain",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sleep",
                ThirdPersonSingular = "Sleeps",
                PresentParticiple = "Sleeping",
                Preterit = new List<string>()
                {
                    "Slept",
                },
                PastParticiple = new List<string>()
                {
                    "Slept",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Slide",
                ThirdPersonSingular = "Slides",
                PresentParticiple = "Sliding",
                Preterit = new List<string>()
                {
                    "Slid",
                },
                PastParticiple = new List<string>()
                {
                    "Slid",
                    "Slidden",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sling",
                ThirdPersonSingular = "Slings",
                PresentParticiple = "Slinging",
                Preterit = new List<string>()
                {
                    "Slung",
                },
                PastParticiple = new List<string>()
                {
                    "Slung",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Slink",
                ThirdPersonSingular = "Slinks",
                PresentParticiple = "Slinking",
                Preterit = new List<string>()
                {
                    "Slunk",
                },
                PastParticiple = new List<string>()
                {
                    "Slunk",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Slit",
                ThirdPersonSingular = "Slits",
                PresentParticiple = "Slitting",
                Preterit = new List<string>()
                {
                    "Slit",
                },
                PastParticiple = new List<string>()
                {
                    "Slit",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Smell",
                ThirdPersonSingular = "Smells",
                PresentParticiple = "Smelling",
                Preterit = new List<string>()
                {
                    "Smelt",
                    "Smelled",
                },
                PastParticiple = new List<string>()
                {
                    "Smelt",
                    "Smelled",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sneak",
                ThirdPersonSingular = "Sneaks",
                PresentParticiple = "Sneaking",
                Preterit = new List<string>()
                {
                    "Sneaked",
                    "Snuck",
                },
                PastParticiple = new List<string>()
                {
                    "Sneaked",
                    "Snuck",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Soothsay",
                ThirdPersonSingular = "Soothsays",
                PresentParticiple = "Soothsaying",
                Preterit = new List<string>()
                {
                    "Soothsaid",
                },
                PastParticiple = new List<string>()
                {
                    "Soothsaid",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sow",
                ThirdPersonSingular = "Sows",
                PresentParticiple = "Sowing",
                Preterit = new List<string>()
                {
                    "Sowed",
                },
                PastParticiple = new List<string>()
                {
                    "Sown",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Speak",
                ThirdPersonSingular = "Speaks",
                PresentParticiple = "Speaking",
                Preterit = new List<string>()
                {
                    "Spoke",
                },
                PastParticiple = new List<string>()
                {
                    "Spoken",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Speed",
                ThirdPersonSingular = "Speeds",
                PresentParticiple = "Speeding",
                Preterit = new List<string>()
                {
                    "Sped",
                    "Speeded",
                },
                PastParticiple = new List<string>()
                {
                    "Sped",
                    "Speeded",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Spell",
                ThirdPersonSingular = "Spells",
                PresentParticiple = "Spelling",
                Preterit = new List<string>()
                {
                    "Spelt",
                    "Spelled",
                },
                PastParticiple = new List<string>()
                {
                    "Spelt",
                    "Spelled",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Spend",
                ThirdPersonSingular = "Spends",
                PresentParticiple = "Spending",
                Preterit = new List<string>()
                {
                    "Spent",
                },
                PastParticiple = new List<string>()
                {
                    "Spent",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Spill",
                ThirdPersonSingular = "Spills",
                PresentParticiple = "Spilling",
                Preterit = new List<string>()
                {
                    "Spilt",
                    "Spilled",
                },
                PastParticiple = new List<string>()
                {
                    "Spilt",
                    "Spilled",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Spin",
                ThirdPersonSingular = "Spins",
                PresentParticiple = "Spinning",
                Preterit = new List<string>()
                {
                    "Span",
                    "Spun",
                },
                PastParticiple = new List<string>()
                {
                    "Spun",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Spit",
                ThirdPersonSingular = "Spits",
                PresentParticiple = "Spitting",
                Preterit = new List<string>()
                {
                    "Spat",
                    "Spit",
                },
                PastParticiple = new List<string>()
                {
                    "Spat",
                    "Spit",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Split",
                ThirdPersonSingular = "Splits",
                PresentParticiple = "Splitting",
                Preterit = new List<string>()
                {
                    "Split",
                },
                PastParticiple = new List<string>()
                {
                    "Split",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Spoil",
                ThirdPersonSingular = "Spoils",
                PresentParticiple = "Spoiling",
                Preterit = new List<string>()
                {
                    "Spoilt",
                    "Spoiled",
                },
                PastParticiple = new List<string>()
                {
                    "Spoilt",
                    "Spoiled",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Spread",
                ThirdPersonSingular = "Spreads",
                PresentParticiple = "Spreading",
                Preterit = new List<string>()
                {
                    "Spread",
                },
                PastParticiple = new List<string>()
                {
                    "Spread",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Spring",
                ThirdPersonSingular = "Springs",
                PresentParticiple = "Springing",
                Preterit = new List<string>()
                {
                    "Sprang",
                },
                PastParticiple = new List<string>()
                {
                    "Sprung",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Stand",
                ThirdPersonSingular = "Stands",
                PresentParticiple = "Standing",
                Preterit = new List<string>()
                {
                    "Stood",
                },
                PastParticiple = new List<string>()
                {
                    "Stood",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Steal",
                ThirdPersonSingular = "Steals",
                PresentParticiple = "Stealing",
                Preterit = new List<string>()
                {
                    "Stole",
                },
                PastParticiple = new List<string>()
                {
                    "Stolen",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Stick",
                ThirdPersonSingular = "Sticks",
                PresentParticiple = "Sticking",
                Preterit = new List<string>()
                {
                    "Stuck",
                },
                PastParticiple = new List<string>()
                {
                    "Stuck",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sting",
                ThirdPersonSingular = "Stings",
                PresentParticiple = "Stinging",
                Preterit = new List<string>()
                {
                    "Stung",
                },
                PastParticiple = new List<string>()
                {
                    "Stung",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Stink",
                ThirdPersonSingular = "Stinks",
                PresentParticiple = "Stinking",
                Preterit = new List<string>()
                {
                    "Stank",
                },
                PastParticiple = new List<string>()
                {
                    "Stunk",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Stride",
                ThirdPersonSingular = "Strides",
                PresentParticiple = "Striding",
                Preterit = new List<string>()
                {
                    "Strode",
                    "Strided",
                },
                PastParticiple = new List<string>()
                {
                    "Stridden",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Strike",
                ThirdPersonSingular = "Strikes",
                PresentParticiple = "Striking",
                Preterit = new List<string>()
                {
                    "Struck",
                },
                PastParticiple = new List<string>()
                {
                    "Struck",
                    "Stricken",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "String",
                ThirdPersonSingular = "Strings",
                PresentParticiple = "Stringing",
                Preterit = new List<string>()
                {
                    "Strung",
                },
                PastParticiple = new List<string>()
                {
                    "Strung",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Strip",
                ThirdPersonSingular = "Strips",
                PresentParticiple = "Stripping",
                Preterit = new List<string>()
                {
                    "Stript",
                    "Stripped",
                },
                PastParticiple = new List<string>()
                {
                    "Stript",
                    "Stripped",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Strive",
                ThirdPersonSingular = "Strives",
                PresentParticiple = "Striving",
                Preterit = new List<string>()
                {
                    "Strove",
                },
                PastParticiple = new List<string>()
                {
                    "Striven",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sublet",
                ThirdPersonSingular = "Sublets",
                PresentParticiple = "Subletting",
                Preterit = new List<string>()
                {
                    "Sublet",
                },
                PastParticiple = new List<string>()
                {
                    "Sublet",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sunburn",
                ThirdPersonSingular = "Sunburns",
                PresentParticiple = "Sunburning",
                Preterit = new List<string>()
                {
                    "Sunburned",
                    "Sunburnt",
                },
                PastParticiple = new List<string>()
                {
                    "Sunburned",
                    "Sunburnt",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Swear",
                ThirdPersonSingular = "Swears",
                PresentParticiple = "Swearing",
                Preterit = new List<string>()
                {
                    "Swore",
                },
                PastParticiple = new List<string>()
                {
                    "Sworn",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sweat",
                ThirdPersonSingular = "Sweats",
                PresentParticiple = "Sweating",
                Preterit = new List<string>()
                {
                    "Sweat",
                    "Sweated",
                },
                PastParticiple = new List<string>()
                {
                    "Sweat",
                    "Sweated",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Sweep",
                ThirdPersonSingular = "Sweeps",
                PresentParticiple = "Sweeping",
                Preterit = new List<string>()
                {
                    "Swept",
                    "Sweeped",
                },
                PastParticiple = new List<string>()
                {
                    "Swept",
                    "Sweeped",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Swell",
                ThirdPersonSingular = "Swells",
                PresentParticiple = "Swelling",
                Preterit = new List<string>()
                {
                    "Swelled",
                },
                PastParticiple = new List<string>()
                {
                    "Swollen",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Swim",
                ThirdPersonSingular = "Swims",
                PresentParticiple = "Swimming",
                Preterit = new List<string>()
                {
                    "Swam",
                },
                PastParticiple = new List<string>()
                {
                    "Swum",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Swing",
                ThirdPersonSingular = "Swings",
                PresentParticiple = "Swinging",
                Preterit = new List<string>()
                {
                    "Swung",
                },
                PastParticiple = new List<string>()
                {
                    "Swung",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Take",
                ThirdPersonSingular = "Takes",
                PresentParticiple = "Taking",
                Preterit = new List<string>()
                {
                    "Took",
                },
                PastParticiple = new List<string>()
                {
                    "Taken",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Teach",
                ThirdPersonSingular = "Teaches",
                PresentParticiple = "Teaching",
                Preterit = new List<string>()
                {
                    "Taught",
                },
                PastParticiple = new List<string>()
                {
                    "Taught",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Tear",
                ThirdPersonSingular = "Tears",
                PresentParticiple = "Tearing",
                Preterit = new List<string>()
                {
                    "Tore",
                },
                PastParticiple = new List<string>()
                {
                    "Torn",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Tell",
                ThirdPersonSingular = "Tells",
                PresentParticiple = "Telling",
                Preterit = new List<string>()
                {
                    "Told",
                },
                PastParticiple = new List<string>()
                {
                    "Told",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Think",
                ThirdPersonSingular = "Thinks",
                PresentParticiple = "Thinking",
                Preterit = new List<string>()
                {
                    "Thought",
                },
                PastParticiple = new List<string>()
                {
                    "Thought",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Thrive",
                ThirdPersonSingular = "Thrives",
                PresentParticiple = "Thriving",
                Preterit = new List<string>()
                {
                    "Throve",
                    "Thrived",
                },
                PastParticiple = new List<string>()
                {
                    "Thriven",
                    "Thrived",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Throw",
                ThirdPersonSingular = "Throws",
                PresentParticiple = "Throwing",
                Preterit = new List<string>()
                {
                    "Threw",
                },
                PastParticiple = new List<string>()
                {
                    "Thrown",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Thrust",
                ThirdPersonSingular = "Thrusts",
                PresentParticiple = "Thrusting",
                Preterit = new List<string>()
                {
                    "Thrust",
                },
                PastParticiple = new List<string>()
                {
                    "Thrust",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Tread",
                ThirdPersonSingular = "Treads",
                PresentParticiple = "Treading",
                Preterit = new List<string>()
                {
                    "Trod",
                },
                PastParticiple = new List<string>()
                {
                    "Trodden",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Undergo",
                ThirdPersonSingular = "Undergoes",
                PresentParticiple = "Undergoing",
                Preterit = new List<string>()
                {
                    "Underwent",
                },
                PastParticiple = new List<string>()
                {
                    "Undergone",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Understand",
                ThirdPersonSingular = "Understands",
                PresentParticiple = "Understanding",
                Preterit = new List<string>()
                {
                    "Understood",
                },
                PastParticiple = new List<string>()
                {
                    "Understood",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Undertake",
                ThirdPersonSingular = "Undertakes",
                PresentParticiple = "Undertaking",
                Preterit = new List<string>()
                {
                    "Undertook",
                },
                PastParticiple = new List<string>()
                {
                    "Undertaken",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Upset",
                ThirdPersonSingular = "Upsets",
                PresentParticiple = "Upsetting",
                Preterit = new List<string>()
                {
                    "Upset",
                },
                PastParticiple = new List<string>()
                {
                    "Upset",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Vex",
                ThirdPersonSingular = "Vexes",
                PresentParticiple = "Vexing",
                Preterit = new List<string>()
                {
                    "Vext",
                    "Vexed",
                },
                PastParticiple = new List<string>()
                {
                    "Vext",
                    "Vexed",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Wake",
                ThirdPersonSingular = "Wakes",
                PresentParticiple = "Waking",
                Preterit = new List<string>()
                {
                    "Woke",
                },
                PastParticiple = new List<string>()
                {
                    "Woken",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Wear",
                ThirdPersonSingular = "Wears",
                PresentParticiple = "Wearing",
                Preterit = new List<string>()
                {
                    "Wore",
                },
                PastParticiple = new List<string>()
                {
                    "Worn",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Weave",
                ThirdPersonSingular = "Weaves",
                PresentParticiple = "Weaving",
                Preterit = new List<string>()
                {
                    "Wove",
                },
                PastParticiple = new List<string>()
                {
                    "Woven",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Wed",
                ThirdPersonSingular = "Weds",
                PresentParticiple = "Wedding",
                Preterit = new List<string>()
                {
                    "Wed",
                    "Wedded",
                },
                PastParticiple = new List<string>()
                {
                    "Wed",
                    "Wedded",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Weep",
                ThirdPersonSingular = "Weeps",
                PresentParticiple = "Weeping",
                Preterit = new List<string>()
                {
                    "Wept",
                },
                PastParticiple = new List<string>()
                {
                    "Wept",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Wend",
                ThirdPersonSingular = "Wends",
                PresentParticiple = "Wending",
                Preterit = new List<string>()
                {
                    "Wended",
                    "Went",
                },
                PastParticiple = new List<string>()
                {
                    "Wended",
                    "Went",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Wet",
                ThirdPersonSingular = "Wets",
                PresentParticiple = "Wetting",
                Preterit = new List<string>()
                {
                    "Wet",
                    "Wetted",
                },
                PastParticiple = new List<string>()
                {
                    "Wet",
                    "Wetted",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Win",
                ThirdPersonSingular = "Wins",
                PresentParticiple = "Winning",
                Preterit = new List<string>()
                {
                    "Won",
                },
                PastParticiple = new List<string>()
                {
                    "Won",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Wind",
                ThirdPersonSingular = "Winds",
                PresentParticiple = "Winding",
                Preterit = new List<string>()
                {
                    "Wound",
                },
                PastParticiple = new List<string>()
                {
                    "Wound",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Withdraw",
                ThirdPersonSingular = "Withdraws",
                PresentParticiple = "Withdrawing",
                Preterit = new List<string>()
                {
                    "Withdrew",
                },
                PastParticiple = new List<string>()
                {
                    "Withdrawn",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Withhold",
                ThirdPersonSingular = "Withholds",
                PresentParticiple = "Withholding",
                Preterit = new List<string>()
                {
                    "Withheld",
                },
                PastParticiple = new List<string>()
                {
                    "Withheld",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Withstand",
                ThirdPersonSingular = "Withstands",
                PresentParticiple = "Withstanding",
                Preterit = new List<string>()
                {
                    "Withstood",
                },
                PastParticiple = new List<string>()
                {
                    "Withstood",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Wring",
                ThirdPersonSingular = "Wrings",
                PresentParticiple = "Wringing",
                Preterit = new List<string>()
                {
                    "Wrung",
                },
                PastParticiple = new List<string>()
                {
                    "Wrung",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Write",
                ThirdPersonSingular = "Writes",
                PresentParticiple = "Writing",
                Preterit = new List<string>()
                {
                    "Wrote",
                },
                PastParticiple = new List<string>()
                {
                    "Written",
                }
            },
            new IrregularVerb()
            {
                BaseForm = "Zinc",
                ThirdPersonSingular = "Zincs",
                PresentParticiple = "Zincking",
                Preterit = new List<string>()
                {
                    "Zinced",
                    "Zincked",
                },
                PastParticiple = new List<string>()
                {
                    "Zinced",
                    "Zincked",
                }
            }
        };
    }
}
