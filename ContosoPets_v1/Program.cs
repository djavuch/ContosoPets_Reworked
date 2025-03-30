using System.Text.RegularExpressions;
using ContosoPets_v1.Utilities;

namespace ContosoPets_v1
{
    internal class Menu
    {
        public static void Main(string[] args)
        {
            PetsDataStorage storage = new PetsDataStorage();
            storage.CreateDemoPetsList();

            string? menuSelection;

            do
            {
                Console.WriteLine("Welcome to the Contoso PetFriends app. Your main menu options are: ");
                Console.WriteLine(" 1. List all of our current pet information");
                Console.WriteLine(" 2. Add a new animal friend to the ourAnimals array");
                Console.WriteLine(" 3. Ensure animal ages and physical descriptions are complete");
                Console.WriteLine(" 4. Ensure animal nicknames and personality descriptions are complete");
                Console.WriteLine(" 5. Edit an pet’s data (name, age, personality and physical description");
                Console.WriteLine(" 6. Remove pet from list");
                Console.WriteLine(" 7. Display all cats with a specified characteristic");
                Console.WriteLine(" 8. Display all dogs with a specified characteristic");
                Console.WriteLine();
                Console.WriteLine("Enter your selection number (or type Exit to exit the program)");


                menuSelection = Console.ReadLine();

                switch (menuSelection)
                {
                    case "1":
                        storage.DisplayPets();
                        break;
                    case "2":
                        storage.AddPet();
                        break;
                    case "3":
                        storage.EnsureAgeAndDescription();
                        break;
                    case "4":
                        storage.EnsureNameAndPersonality();
                        break;
                    case "5":
                        storage.EditPetsData("");
                        break;
                    case "6":
                        storage.RemovePet("");
                        break;
                    case "7":
                        storage.SearchPetsBySpecies("cat");
                        break;
                    case "8":
                        storage.SearchPetsBySpecies("dog");
                        break;
                }
            } while (menuSelection != "Exit".ToLower());
            
        }
        public class PetsData
        {
            private string _petAge = "?";
            private decimal _suggestedDonation;
            private readonly string _PetId;
            public string? PetSpecies { get; private set; }
            public string? PetId
            {
                get { return _PetId; }
            }
            public string? PetAge
            {
                get => _petAge;
                set
                {
                    if (value == "?" || int.TryParse(value, out int age) && age >= 0)
                        _petAge = value;
                    else if (!int.TryParse(value, out _))
                        throw new ArgumentException("Age value must be included only numbers or '?'.");
                    else
                        throw new ArgumentException("Age value must be non-negative number or '?'.");
                }
            }
            public string? PetPhysicalDescription;
            public string? PetPersonalityDescription;
            public string? PetName;
            public decimal SuggestedDonation
            {
                get => _suggestedDonation;
                set
                {
                    if (value < 0)
                        throw new ArgumentException("Suggested donation must be a non-negative number.");
                    _suggestedDonation = value;
                }
            }
            public PetsData(string species, string petId)
            {
                PetSpecies = species;
                this._PetId = petId;
            }

            public override string ToString()
            {
                return $"Species: {PetSpecies}\n" + $"ID #: {PetId}\n" +
                       $"Age: {PetAge}\n" + $"Nickname: {PetName}\n" + $"Physical Description: {PetPhysicalDescription}\n" +
                       $"Personality Description: {PetPersonalityDescription}\n" +
                       $"Suggested Donation: {SuggestedDonation.ToString("C", GlobalSettings.Culture)}\n";
            }
        }

        public class PetsDataStorage
        {
            private List<PetsData> _petList = new List<PetsData>();

            public void DisplayPets()
            {
                if (_petList == null || _petList.Count == 0)
                {
                    Console.WriteLine("No pets are currently available.");
                    InputHandler.InputData("Press the Enter key to continue.");
                    return;
                }

                foreach (var pet in _petList)
                {
                    Console.WriteLine(pet.ToString());
                }
                InputHandler.InputData("Press the Enter key to continue.");
            }

            public void AddPet()
            {
                string anotherPet = "y";
                int petCount = 0;
                int maxPets = 7;

                // Checking possible limits
                foreach (var pet in _petList)
                {
                    if (!string.IsNullOrEmpty(pet.PetId))
                    {
                        petCount += 1;
                    }
                }

                if (petCount < maxPets)
                {
                    Console.WriteLine($"We currently have {petCount} pets that need homes. We can manage {(maxPets - petCount)} more.");
                }
                // Start adding
                while (anotherPet == "y" && petCount < maxPets)
                {
                    bool validEntry = false;
                    string inputPetSpecies = "";
                    string inputPetName = "";
                    string inputPetAge = "";
                    string inputPetPhysicalDescription = "";
                    string inputPetPersonalityDescription = "";
                    decimal inputSuggestedDonation = 0;
                    // Checking the pet's species
                    do
                    {
                        Console.WriteLine();
                        inputPetSpecies = InputHandler.InputData("\n\rEnter 'dog' or 'cat' to begin new entry: \nPlease note, our shelter only accepts cats and dogs!").ToLower();
                        if (inputPetSpecies != null)
                        {
                            validEntry = (inputPetSpecies != "dog" && inputPetSpecies != "cat") ? false : true;
                        }
                    } while (!validEntry);
                    // Creating a temporary class
                    PetsData pet = new PetsData(inputPetSpecies, "");
                    // Checking pet's age
                    do
                    {
                        inputPetAge = InputHandler.InputData("\rEnter the pet's age or enter '?' if unknown: ");
                        try
                        {
                            pet.PetAge = inputPetAge;
                            validEntry = true;
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            validEntry = false;
                        }
                    } while (!validEntry);
                    // Input pet's name
                    inputPetName = InputHandler.InputData("Enter the pet's name: ").ToLower();
                    if (inputPetName != null)
                    {
                        inputPetName = inputPetName == "" ? "tbd" : inputPetName;
                    }
                    // Input pet's physical description 
                    inputPetPhysicalDescription = InputHandler.InputData("Enter a physical description of the pet (size, color, gender, weight, housebroken): ");
                    if (inputPetPhysicalDescription != null)
                    {
                        inputPetPhysicalDescription = inputPetPhysicalDescription == "" ? "tbd" : inputPetPhysicalDescription;
                    }
                    // Input pet's personality description
                    inputPetPersonalityDescription = InputHandler.InputData("Enter a personality description of the pet's personality (likes or dislikes, tricks, energy level): ");
                    if (inputPetPersonalityDescription != null)
                    {
                        inputPetPersonalityDescription = inputPetPersonalityDescription == "" ? "tbd" : inputPetPersonalityDescription;
                    }
                    // Input a possible donation
                    inputSuggestedDonation = InputHandler.InputDecimal("Enter the suggested donation amount or '0': ");

                    // Create a new animal object in list
                    PetsData newAnimal = new PetsData(
                        inputPetSpecies,
                        inputPetSpecies.Substring(0, 1) + (petCount + 1).ToString()
                    )
                    {
                        PetName = inputPetName,
                        PetAge = inputPetAge,
                        PetPhysicalDescription = inputPetPhysicalDescription,
                        PetPersonalityDescription = inputPetPersonalityDescription,
                        SuggestedDonation = inputSuggestedDonation
                    };

                    _petList.Add(newAnimal);

                    petCount += 1;
                    // Check maxPet limit after adding
                    if (petCount < maxPets)
                    {
                        do
                        {
                            anotherPet = InputHandler.InputData("Do you want to enter another pet? (y/n): ");
                            if (anotherPet != null)
                            {
                                anotherPet.ToLower();
                            }
                        } while (anotherPet != "y" && anotherPet != "n");
                    }
                }
                if (petCount >= maxPets)
                {
                    Console.WriteLine("We have reached our maximum capacity for pets. Thank you for considering adoption!");
                    InputHandler.InputData("Press the Enter key to continue.");
                }
            }

            public void EnsureAgeAndDescription()
            {
                // Checking if the age and physical description fields are complete
                bool ageComplete = false;
                bool descriptionComplete = false;
                // Loop through the list of pets
                foreach (var pet in _petList)
                {
                    if (pet.PetAge == "?")
                    {
                        do
                        {
                            pet.PetAge = InputHandler.InputData($"Enter an age for pet with Id {pet.PetId}: ");
                            if (pet.PetAge != null)
                            {
                                if (pet.PetAge != "?")
                                    ageComplete = int.TryParse(pet.PetAge, out int age);
                                else
                                    ageComplete = false;
                            }
                        } while (!ageComplete);
                    }

                    Console.WriteLine($"Physical description for {pet.PetId}: " + pet.PetPhysicalDescription);

                    if (string.IsNullOrEmpty(pet.PetPhysicalDescription) || pet.PetPhysicalDescription == "tbd")
                    {
                        do
                        {
                            pet.PetPhysicalDescription = InputHandler.InputData($"Enter a physical description for pet with Id {pet.PetId}: ");
                            if (pet.PetPhysicalDescription != null)
                            {
                                descriptionComplete = !string.IsNullOrEmpty(pet.PetPhysicalDescription) && pet.PetPhysicalDescription != "tbd";
                            }
                            if (descriptionComplete)
                            {
                                Console.WriteLine($"Physical description for {pet.PetId} " + pet.PetPhysicalDescription);
                            }
                        } while (!descriptionComplete);
                    }
                }
                if (ageComplete == true && descriptionComplete == true)
                {
                    Console.WriteLine("Age and Physical description fields are complete for all of our friends.");
                    InputHandler.InputData("Press the Enter key to continue.");
                }
            }

            public void EnsureNameAndPersonality()
            {
                bool nameComplete = false;
                bool personalityComplete = false;

                foreach (var pet in _petList)
                {
                    if (string.IsNullOrEmpty(pet.PetName) || pet.PetName == "tbd")
                    {
                        do
                        {
                            pet.PetName = InputHandler.InputData($"Enter a nickname for pet with Id {pet.PetId}: ");
                            if (pet.PetName != null)
                            {
                                nameComplete = !string.IsNullOrEmpty(pet.PetName) && pet.PetName != "tbd";
                            }
                        } while (!nameComplete);
                    }

                    Console.WriteLine($"Personality description for {pet.PetId}: " + pet.PetPersonalityDescription);
                    if (string.IsNullOrEmpty(pet.PetPersonalityDescription) || pet.PetPersonalityDescription == "tbd")
                    {
                        do
                        {
                            pet.PetPersonalityDescription = InputHandler.InputData($"Enter a personality description for pet with Id {pet.PetId}");
                            if (pet.PetPersonalityDescription != null)
                            {
                                personalityComplete = !string.IsNullOrEmpty(pet.PetPersonalityDescription) && pet.PetPersonalityDescription != "tbd";
                            }
                            if (personalityComplete)
                            {
                                Console.WriteLine($"Personality description for {pet.PetId}" + pet.PetPersonalityDescription);
                            }
                        } while (!personalityComplete);
                    }
                }
                if (nameComplete == true && personalityComplete == true)
                {
                    Console.WriteLine("Nickname and Personality description fields are complete for all of our friends.");
                    Console.WriteLine();
                    InputHandler.InputData("Press the Enter key to continue.");
                }
            }

            public void EditPetsData(string petId)
            {
                // Edit the pet's data
                string editSelection = "";
                string continueEdit = "y";
                // Validate the pet's ID
                petId = IdsInputCheck.GetValidatedPetId(_petList);
                // Search the pet ID if validation is complete    
                var pet = _petList.FirstOrDefault(pet => pet.PetId == petId);

                do
                {
                    if (continueEdit == "y")
                    {
                        editSelection = InputHandler.InputData($"Enter one of the proposed options to continue working with our database: 'name', 'age', 'physical' or 'personal': ");
                    }
                    else
                    {
                        editSelection = InputHandler.InputData($"Pet with {petId} found. To edit data, enter one of the values ​​below: 'name', 'age', 'physical' or 'personal': ");
                    }
                    // Menu for editing pet data
                    switch (editSelection)
                    {
                        case "name":
                            pet.PetName = CharInputCheck.GetValidatedCharInput("Enter the pet's name: ");
                            break;

                        case "age":
                            bool validEntry = false;
                            do
                            {
                                string inputPetAge = InputHandler.InputData("Enter the pet's age or enter '?' if unknown: ");
                                try
                                {
                                    pet.PetAge = inputPetAge;
                                    validEntry = true;
                                    Console.WriteLine($"The pet's age for Id {petId} has been updated.");
                                }
                                catch (ArgumentException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    validEntry = false;
                                }
                                ;
                            } while (!validEntry);
                            break;

                        case "physical":
                            pet.PetPhysicalDescription = InputHandler.InputData("Enter the pet's physical description: ");
                            Console.WriteLine($"The pet's physical description for Id {petId} has been updated.");
                            break;

                        case "personal":
                            pet.PetPersonalityDescription = InputHandler.InputData("Enter the pet's personality description: ");
                            Console.WriteLine($"The pet's personality description for Id {petId} has been updated.");
                            break;

                        default:
                            Console.WriteLine("Invalid selection. Please try again.");
                            break;
                    }
                    continueEdit = InputHandler.InputData("Do you want to edit another field? (y/n): ").Trim().ToLower();
                    while (continueEdit != "y" && continueEdit != "n")
                    {
                        Console.WriteLine("Invalid input. Please enter 'y' to continue or 'n' to exit.");
                        continueEdit = InputHandler.InputData("Do you want to edit another field? (y/n): ").Trim().ToLower();
                    }
                } while (continueEdit == "y");
            }

            public void RemovePet(string petId)
            {
                string continueRemove = "y";
                petId = IdsInputCheck.GetValidatedPetId(_petList);

                var petToRemove = _petList.Single(pet => pet.PetId == petId);
                do
                {
                    _petList.Remove(petToRemove);
                    Console.WriteLine($"Pet with ID {petId} was removed from the list.");

                    continueRemove = InputHandler.InputData("Do you want to remove another pet from list? (y/n): ").Trim().ToLower();
                    while (continueRemove != "y" && continueRemove != "n")
                    {
                        Console.WriteLine("Invalid input. Please enter 'y' to continue or 'n' to exit.");
                        continueRemove = InputHandler.InputData("Do you want to remove another pet from list? (y/n): ").Trim().ToLower();
                    }
                } while(continueRemove == "y");
            }

            public void SearchPetsBySpecies(string species)
            {
                bool matchesAnyPet = false;

                string petCharacteristics = "";

                while (string.IsNullOrWhiteSpace(petCharacteristics))
                {
                    petCharacteristics = InputHandler.InputData("Enter pet's characteristics to search separated by commas: ");
                }

                string[] searchTerms = petCharacteristics.Split(',')
                                                         .Select(term => term.Trim())
                                                         .ToArray();
                // rotating search icons
                string[] searchingIcons = { " |", " /", "--", " \\", " *" };
                // loop through the list of pets
                foreach (var pet in _petList.Where(pet => pet.PetSpecies == species))
                {
                    bool matchesCurrentPet = false;
                    // loop through the search terms
                    foreach (var term in searchTerms)
                    {
                        // rotating search icons
                        for (int i = 2; i > -1; i--)
                        {
                            foreach (string icon in searchingIcons)
                            {
                                Console.Write($"\rSearching for our {pet.PetSpecies} {pet.PetName} for {term.Trim()} {icon} {i.ToString()}");
                                Thread.Sleep(100);
                            }
                            Console.Write($"\r{new string(' ', Console.BufferWidth)}");
                        }
                        // Check if the search term is in the pet's physical or personality description
                        if (pet.PetPhysicalDescription.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                            pet.PetPersonalityDescription.Contains(term, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"\nOur {pet.PetSpecies} {pet.PetName} is a match for {term}!");
                            matchesCurrentPet = true;
                            matchesAnyPet = true;
                        }
                    }
                    // Display a message if pet's matching the description are found
                    if (matchesCurrentPet)
                    {
                        Console.WriteLine($"\n\rName: {pet.PetName} ({pet.PetId})\nPhysical description: {pet.PetPhysicalDescription}\nPersonal description: {pet.PetPersonalityDescription}\n");
                    }
                }
                // Message output if no pet's matching the description are found
                if (!matchesAnyPet)
                {
                    Console.WriteLine($"\nNone of our {species} are a match found for: " + petCharacteristics);
                }
                InputHandler.InputData("Press the Enter key to continue.");
            }

            public void CreateDemoPetsList()
            {
                _petList = new List<PetsData>
                { 
                    new PetsData("dog", "d1")
                    {
                        PetAge = "2",
                        PetPhysicalDescription = "medium sized cream colored female golden retriever weighing about 65 pounds. housebroken.",
                        PetPersonalityDescription = "loves to have her belly rubbed and likes to chase her tail. gives lots of kisses.",
                        PetName = "Lola",
                        SuggestedDonation = 85.00m
                    },
                    new PetsData("dog", "d2")
                    {
                        PetAge = "9",
                        PetPhysicalDescription = "large reddish-brown male golden retriever weighing about 85 pounds. housebroken.",
                        PetPersonalityDescription = "loves to have his ears rubbed when he greets you at the door, or at any time! loves to lean-in and give doggy hugs.",
                        PetName = "Loki",
                        SuggestedDonation = 49.99m
                    },
                    new PetsData("cat", "c3")
                    {
                        PetAge = "1",
                        PetPhysicalDescription = "small white female weighing about 8 pounds. litter box trained.",
                        PetPersonalityDescription = "friendly",
                        PetName = "Puss",
                        SuggestedDonation = 40.00m
                    },
                    new PetsData("cat", "c4")
                    {
                        PetAge = "?",
                        PetPhysicalDescription = "",
                        PetPersonalityDescription = "",
                        PetName = "",
                        SuggestedDonation = 0.00m
                    }
                };
            }

            public IReadOnlyList<PetsData> GetPets()
            {
                return _petList.AsReadOnly();
            }
        }
    }
}