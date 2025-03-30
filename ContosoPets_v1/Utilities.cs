using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace ContosoPets_v1.Utilities
{
    public static class GlobalSettings
    {
        public static readonly CultureInfo Culture = new CultureInfo("en-US");
    }
    // Input handler for user input
    public static class InputHandler
    {
        public static string InputData(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine() ?? string.Empty;

        }
        public static decimal InputDecimal(string prompt)
        {
            string? input;

            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrEmpty(input) && decimal.TryParse(input, out decimal value))
                {
                    return value;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            } while (true);
        }
    }
    // Checking if numbers have been entered correctly
    public static class NumbersInputCheck
    {
        public static bool ContainsLetters(string input)
        {
            return Regex.IsMatch(input, @"[a-zA-Z]");
        }

        public static string GetValidatedNumberInput(string prompt)
        {
            string input;
            bool isValid;

            do
            {
                input = InputHandler.InputData(prompt) ?? string.Empty;

                isValid = !ContainsLetters(input);

                if (!isValid)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            } while (!isValid);

            return input;
        }
    }
    // Checking if characters have been entered correctly
    public static class CharInputCheck
    {
        public static bool ContainsNumbers(string prompt)
        {
            return Regex.IsMatch(prompt, @"[0-9]");
        }

        public static string GetValidatedCharInput(string prompt)
        {
            string input;
            bool isValid;

            do
            {
                input = InputHandler.InputData(prompt) ?? string.Empty;
                isValid = !ContainsNumbers(input);
                if (!isValid)
                {
                    Console.WriteLine("Invalid input. Only characters allowed.");
                }
            } while (!isValid);

            return input;
        }
    }

    public static class IdsInputCheck
    {
        internal static string GetValidatedPetId(List<Menu.PetsData> petList)
        {
            string petId;
            bool isPetIdValid = false;

            do
            {
                petId = InputHandler.InputData("Enter the pet's ID to edit (e.g., d1 or c1): ")?.Trim() ?? "";

                if (Regex.IsMatch(petId, @"^[dc]\d+$"))
                {
                    isPetIdValid = petList.Any(pet => pet.PetId == petId);

                    if (!isPetIdValid)
                    {
                        Console.WriteLine($"ID {petId} not found. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid ID format. Please enter a valid ID.");
                }

            } while (!isPetIdValid);

            return petId;
        }

    }
}