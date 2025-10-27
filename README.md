# String Calculator (F#)

This project implements a feature-rich String Calculator using the functional programming paradigm in F#.
Each task was approached as a learning milestone, emphasizing incremental development, problem analysis, and reflective thinking rather than only reaching a final solution.

Throughout the process, I focused on:

-Building clean and readable code.

-Applying functional programming principles (immutability, composability, recursion avoidance with iteration for clarity).

-Avoiding regular expressions to improve manual string parsing skills.

-Reflecting after every stage to consolidate understanding.

Note: Certain tasks were addressed in earlier stages, and therefore share the same code implementation. Nevertheless, this report focuses on the unique challenges and learning outcomes associated with each task.

# Functional Programming Journey in F#
When I started this project, my main goal wasn’t just to make a program work — it was to understand how to think functionally.
Until this challenge, most of my experience had been with imperative programming, where the focus is on how to do things step by step.

Through the String Calculator project, I learned to focus instead on what the program should do — describing transformations, ensuring purity, and writing predictable, testable functions.

This text will summarizes my personal learning journey in functional programming through nine incremental tasks, describing how my mindset evolved from procedural problem-solving to functional reasoning.

1. At first, my instinct was to think procedurally:
split the string, parse numbers, sum them.

But even in Task 1 and Task 2, I started noticing something:
each operation in F# — like Split, Array.map, or Array.sum — is itself a function that transforms data.

Instead of manually mutating variables, I began to chain transformations, This was my first real exposure to thinking in functions rather than instructions.

Thinking in Purity and Predictability

Functional programming introduced me to the idea of pure functions —
functions that always give the same result for the same input and have no side effects.

My Add() function never prints, never reads files, and never depends on global state.
It simply takes a string and returns an integer.

That small shift changed everything: I began designing code to be predictable, testable, and modular.
When I realized that purity makes debugging easier and composition safer, I started truly appreciating the functional mindset.

2. Purity and Predictability

The Add() function is pure — it depends only on its input and always produces the same output.
This made testing simple and behavior predictable, reinforcing the idea that functions are reliable when isolated.

3. Immutability as Stability

Even when I used mutable variables for simplicity, I understood that state changes should be local and contained.
Immutability gave my code stability and made reasoning about results much easier.

4. Composition and Modularity

As the calculator grew, I learned to divide logic into smaller functions — one for parsing, one for summing, one for validation.
Functional composition made my program cleaner, reusable, and easier to extend with new features.

5. No Regex, Just Logic

Avoiding regular expressions forced me to build manual string parsers using simple character scanning.
It was a challenge, but it improved my attention to detail and my understanding of pure data manipulation.

6. Data Flow Over Control Flow

I learned that functional programming is about how data moves, not how the program runs.
Instead of controlling loops, I designed transformations — letting the data “flow” through functions naturally.

7. Incremental Evolution

Each task expanded on the previous one without breaking it.
That’s when I realized the true power of functional design: when functions are pure, they are easy to extend and reuse.
----------------------------------------------------


This challenge changed the way I think about programming.
I learned that functional programming is not just about syntax — it’s about clarity, predictability, and simplicity.
It taught me to:

-Build pure, reusable functions.

-Keep side effects isolated (only in main).

-Design code that is safe to evolve.

What began as a calculator became an exercise in logic, composition, and disciplined thinking.
F# helped me see programming as a way to express transformations — cleanly, clearly, and with confidence.

# Task Reflection
Task 1 – Handling Empty Inputs and Two Values

At first, I focused on understanding how F# manages input and simple string operations.

The main challenge was learning how F# handles null and empty strings, and how to return a value instead of causing an error.

I discovered String.IsNullOrWhiteSpace, which let me handle empty input gracefully.
I also learned to use Split(',') and Int32.TryParse safely.

This first step taught me how to think about edge cases before coding and to make the function pure and predictable.

Task 2 – Supporting Unlimited Inputs

Once I could handle two numbers, I realized the logic shouldn’t change — only the way I iterate over the input.
I shifted my mindset from fixed-size input to scalable processing.

I had to understand arrays and iteration in F#.
The syntax and immutability rules were new to me, so I needed to learn how to loop correctly.

I replaced my truncation logic with a simple for loop that processed all numbers dynamically.
I also saw that functional programs often handle collections by transforming them, not by manually managing indexes.
This helped me internalize the idea of data flow instead of control flow.

Task 3 – Adding Newline Delimiters

Now I had to make the calculator more flexible — recognizing both commas and newlines as delimiters.
I began thinking in terms of multiple valid separators rather than one fixed rule.

The main challenge was differentiating between a real newline (\n) and a literal string ("\\n").
It took experimentation to understand how F# interprets escape characters.

I expanded the delimiters array to include both real and literal newlines.
I learned to test carefully with different kinds of input.
This task strengthened my debugging skills and my ability to analyze input representation vs. actual value.

Task 4 – Custom Delimiters

This task introduced string parsing logic — recognizing when a string starts with "//" and extracting the delimiter definition.
I began thinking in smaller steps: detect, slice, and transform.

The hardest part was finding the boundary between the delimiter and the numbers.
At first, I misunderstood how Substring worked and caused out-of-range errors.

I wrote a helper function parseCustom to isolate the header.
By breaking the problem into smaller, single-purpose pieces, I learned the power of modularity and composability in functional design.

Task 5 – Handling Negative Numbers

For the first time, I had to handle invalid data.
This made me think about input validation as part of functional purity — errors should be predictable and meaningful.

I wasn’t sure how to “throw” exceptions properly in F#.
I had to understand how functional languages handle errors differently than imperative ones.

I used an ArgumentException and collected all negative values in a list.
This taught me to separate normal data flow from error flow clearly.
I realized that functional programming doesn’t hide errors — it makes them explicit.

Task 6 – Ignoring Numbers Greater Than 1000

This seemed simple at first, but I wanted to do it cleanly.
I didn’t want to filter numbers twice or modify data unnecessarily.

I had to add this rule without making the function more complex.
The challenge was to keep the code readable and declarative.

I simply added a condition: elif value <= 1000 then total <- total + value.
This reinforced how functional design can evolve without breaking prior behavior — just by adding another transformation rule.

Task 7 – Variable-Length Delimiters

Now delimiters could be anything, like "***".
I needed to recognize [ ... ] sections dynamically — this required better parsing logic.

The problem was extracting content between brackets without using regex (which was not allowed).
I initially overcomplicated it by trying multiple splits.

I realized I could use Substring and IndexOf to find the start and end of each [ ... ] section.
This experience made me more confident with manual parsing and string boundaries.
I also started appreciating how functional programming values precision and simplicity over shortcuts.

 Task 8 – Multiple Delimiters (No Regex)

This was the most complex part: supporting many delimiters like [***], [%], [@@], all at once.
I needed a way to loop through the header and extract them manually.

Avoiding regex meant I had to simulate a parser by hand.
At first, I got stuck trying to handle multiple [ ... ] blocks cleanly.

I used a simple while loop to scan through the header, detect every opening [ and closing ], and store each delimiter in a list.
It was more manual but also more educational.
I finally understood how parsing logic truly works at the character level, which gave me a strong confidence boost.

Task 9 – Complex Delimiters (Multiple Long Ones)

By this point, I wanted to verify that my implementation could already handle the example "//[***][%%]\n1***2%%3".
I realized Task 8 had naturally prepared me for this.

No coding was needed — the challenge was verifying correctness and understanding why it already worked.

Testing confirmed that the manual scanning logic handled any number of delimiters, of any length.
I saw how earlier design decisions — especially modularity and clarity — made future extensions effortless.


This project transformed the way I think about programming.
Initially, I focused on what to code — now I focus on how data flows.
I learned that functional programming isn’t about syntax; it’s about clarity, predictability, and composition.

Each challenge taught me something new:

-Handle input safely before processing.

-Think in transformations, not in steps.

-Write pure, testable, and composable functions.

-Isolate errors and side effects cleanly.

-Design code that evolves naturally without rewriting.

What began as a simple string calculator became an exploration of functional problem-solving —
learning not just how to code, but how to think.