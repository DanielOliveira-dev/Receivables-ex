# Receivables
## Made by Daniel Oliveira

### Assumptions 
- Reference is unique for either Receivables and Debtors
- Reference is composed only by numbers
- One Debtor can have Multiple Receivables
- One Receivable can only have one Debtor
- In case a Receivable with the same reference is added, the existing one is updated
- An open invoice is a Receivable which is not cancelled and has no closed date
- A closed invoice is a Receivable which is not cancelled and has a closed date
- Summary statistics are the sum of the paid values for each invoice type
- For this solution, only AU, CH and PT are accepted as country codes
- For this solution, only EUR, CHF and GBP are accepted as currency codes
- AddReceivables will accept one or many Receivables
- Paid Value has to be lower or equal to Opening Value
- Opening Date is always lower or equal then Due Date
- Due Date is always lower or equal to Closed Date
- Date are only accepted in the formats: "dd-MM-yyyy" and "dd/MM/yyyy"

### Time
- It took about 4 hours to effectively do this API
- Most of the time was wasted on the database configuration

### Things that could have been done better
- In the tests project I made a quick builder just to show the implementation of the pattern
- It might look confusing due to the usage of different libraries, just to show the know-how

### Code
- Onion architecture
- API has a reference to Service layer, this one can interact with Domain and Data layers
- DTO validated according to assumptions
- Entity FrameworkCore used as ORM
- AutoMapper to converts DTO into Domain mode
- Projects separated by responsabilities
- The code lacks comments because of its readability
- No logging since it wasn't asked and for this kind of exercise, personally it looked like expendable
- No extra features were added not to over complicate it
- Not many different HTTP responses not to over complicate it

### Tests
- Used Builder pattern
- Autofixture to generate properties
- FluentAssertions for readable assertions
- xUnit
- Moq for mocking repository calls
- Repository and Controllers were not tested since most of scenarios are covered by the service tests.