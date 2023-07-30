using System.Collections.Generic;
using System;
using System.Linq;

namespace Phase2
{
    public class Transition
    {

        public char inputSymbol { get; set; }
        public char[] popSymbol { get; set; }
        public char[] pushSymbol { get; set; }
        public State nextState { get; set; }
        public Transition(char inputsymbol, char[] popSymbol, char[] pushSymbol, State nextState)
        {
            inputSymbol = inputsymbol;
            this.popSymbol = popSymbol;
            this.pushSymbol = pushSymbol;
            this.nextState = nextState;
        }
    }
    public class State
    {
        public string stateName { get; set; }
        public List<Transition> Transitions { get; set; }
        public State(string statename)
        {
            stateName = statename;
            Transitions = new List<Transition>();
        }

        public static State initial { get; set; }
        public static List<State> Finals { get; set; }
    }

    public class Program
    {
        static void Main()
        {
            Stack<char> Stack = new Stack<char>();
            Stack.Push('$');


            List<State> StateList = new List<State>();
            List<string> StackAlphabet = new List<string>();
            string[] StackAlphabet1 = StackAlphabet.ToArray();
            List<String> InputAlpahbet = new List<string>();
            string[] Alphabet1 = StackAlphabet.ToArray();
            List<State> Finals = new List<State>();

            Initialize(ref StateList, ref StackAlphabet1, ref Alphabet1, ref Finals);

            State.initial = StateList[0];
            State.Finals = Finals;

            string input = Console.ReadLine();

            bool Check = false;

            int sum = 0;


            IsAccepted(Stack, State.initial, input, ref Check, sum);

            if (Check)
            {
                Console.WriteLine("Accepted");
            }
            else
            {
                Console.WriteLine("Rejected");
            }



        }

        public static void IsAccepted(Stack<char> Stack, State CurrentState, string Input, ref bool Check, int sum)
        {
            List<char> Pop = new List<char>();
            List<char> Push = new List<char>();

            if (Input == "#" && State.Finals.Contains(CurrentState))
            {
                Check = true;
                return;
            }
            if (Input.Count() == 0 && State.Finals.Contains(CurrentState))
            {
                Check = true;
                return;
            }

            for (int i = 0; i < CurrentState.Transitions.Count; i++)
            {
                Pop = new List<char>();

                if (Input.Count() == 0)
                {
                    if (State.Finals.Contains(CurrentState))
                    {
                        Check = true;
                        return;
                    }
                }
                else
                {

                    if (CurrentState.Transitions[i].inputSymbol == Input[0])
                    {
                        if (Stack.Count() != 0)
                        {

                            int CanPop = PopFromStack(ref Push, CurrentState.Transitions[i].popSymbol, ref Stack);

                            if (CanPop == 1)
                            {

                                Pop = PushToStack(CurrentState.Transitions[i].pushSymbol, ref Stack).ToList();




                                if (Input.Count() >= 1)
                                {

                                    string CurrentInput = Input.Remove(0, 1);
                                    IsAccepted(Stack, CurrentState.Transitions[i].nextState, CurrentInput, ref Check, sum);
                                    if (Check)
                                    {
                                        return;
                                    }

                                    for (int j = 0; j < Pop.Count(); j++)
                                    {
                                        Stack.Pop();
                                    }

                                    if (CanPop == 1)
                                    {
                                        for (int j = Push.Count - 1; j >= 0; j--)
                                        {
                                            Stack.Push(Push[j]);
                                        }
                                    }

                                }

                            }

                            else if (CanPop == -1)
                            {
                                continue;
                            }
                        }
                    }
                }

                if (CurrentState.Transitions[i].inputSymbol == '#')
                {
                    if (Stack.Count() != 0)
                    {

                        int CanPop = PopFromStack(ref Push, CurrentState.Transitions[i].popSymbol, ref Stack);

                        if (CanPop == 1)
                        {

                            Pop = PushToStack(CurrentState.Transitions[i].pushSymbol, ref Stack).ToList();



                            if (Input.Count() >= 0 && sum <= 100)
                            {
                                string CurrentInput = Input;
                                sum = sum + 1;
                                IsAccepted(Stack, CurrentState.Transitions[i].nextState, CurrentInput, ref Check, sum);

                                if (Check)
                                {
                                    return;
                                }

                                for (int j = 0; j < Pop.Count(); j++)
                                {
                                    Stack.Pop();
                                }

                                if (CanPop == 1)
                                {
                                    for (int j = Push.Count - 1; j >= 0; j--)
                                    {
                                        Stack.Push(Push[j]);
                                    }
                                }
                            }


                        }

                        else if (CanPop == -1)
                        {
                            continue;
                        }
                    }
                }
            }
            return;
        }
        public static int PopFromStack(ref List<char> push, char[] PopSymbol, ref Stack<char> stack)
        {
            push = new List<char>();
            int SumPop = 0;
            int SumLambda = 0;
            bool flag = false;

            for (int i = 0; i < PopSymbol.Length; i++)
            {
                if (PopSymbol[i] != '#')
                {
                    if (stack.Peek() == PopSymbol[i])
                    {
                        SumPop++;

                        push.Add(stack.Pop());
                    }
                    else flag = true;
                }
                else SumLambda++;
                if (flag)
                {
                    for (int j = push.Count - 1; j >= 0; j--)
                    {
                        stack.Push(PopSymbol[j]);

                        if (j == 0)
                        {
                            return -1;
                        }
                    }

                }


            }
            if (SumPop + SumLambda == PopSymbol.Length)
            {
                return 1;
            }

            return -1;


        }

        public static char[] PushToStack(char[] PushSymbol, ref Stack<char> stack)
        {
            List<char> popagain = new List<char>();

            for (int i = PushSymbol.Length - 1; i >= 0; i--)
            {
                if (PushSymbol[i] != '#')
                {
                    popagain.Add(PushSymbol[i]);
                    stack.Push(PushSymbol[i]);
                }


            }

            return popagain.ToArray();
        }

        public static void Initialize(ref List<State> States, ref string[] pdaAlphabet, ref string[] stackAlphabet, ref List<State> finals)
        {
            string[] inputstates = Console.ReadLine().Split(new char[] { ' ', '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries);
            States = new List<State>();

            for (int i = 0; i < inputstates.Length; i++)
            {
                State state = new State(inputstates[i]);
                if (i == 0)
                {
                    State.initial = state;
                }
                States.Add(state);
            }

            pdaAlphabet = Console.ReadLine().Split(new char[] { ' ', '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries);
            pdaAlphabet.Append("$");

            stackAlphabet = Console.ReadLine().Split(new char[] { ' ', '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries);

            string[] inputfinals = Console.ReadLine().Split(new char[] { ' ', '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries);


            finals = new List<State>();

            for (int i = 0; i < inputfinals.Length; i++)
            {
                finals.Add(States.Find(state => state.stateName == inputfinals[i]));
            }

            int numTransitions = int.Parse(Console.ReadLine());
            for (int i = 0; i < numTransitions; i++)
            {
                string[] trans = Console.ReadLine().Split(new char[] { ' ', '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);


                Transition transition = new Transition(char.Parse(trans[1]), trans[2].ToCharArray(), trans[3].ToCharArray(), States.Find(state => state.stateName == trans[4]));

                States.Find(state => state.stateName == trans[0]).Transitions.Add(transition);
            }



            // return (States, pdaAlphabet, stackAlphabet, finals);
        }
    }
}