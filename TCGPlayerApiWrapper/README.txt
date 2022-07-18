This program wraps the TCGPlayer API (https://docs.tcgplayer.com/docs), specifically the MTG side of it
It allows a user to track their MTG collection using MongoDB, either by importing a list of scanned cards from the TCGPlayer app, or by manually entering cards

TODOs:
- Add config file with DB connection string, api creds, etc.
- Add base service interface that contains the API and DB access initializations
- Figure whether we can create a DB connection on base interface without keeping a constant connection open
- Create locally-accessible API with this project backing