# User and Transaction Controller

# Authorization 
All controllers are restricted to authorize users only. This ensures secure access and enable treacking of each user's activities via their userId.

# Contollers
 1.  Identity Controller which consist register and login endpoint
 2.  Generate Transaction which consist create transaction and calculate percentage increment endpoin

# Register User
function: Add new users to the database
purpose: Onboard users into the system
Access: allow anonymous

# Login User
function: Authenticates users attempting to perform transaction
Purpose: Verifies identity and enables activity tracking
Access: Requires authentication

# Create Transaction
function: Generates auto-transactions for authorized user base on his/her desire
purpose: Ensures only verified user can initiate transaction related operations
access: Require authentication

# Percentage Increment
function: Check for high value transaction of previoud and current months base on category filters for a specific userId.
purpose: Indentifies significant transaction
Execution: Intended for cron-based authomation, but currently triggeered manually
Access: Requires authentication


   
