CREATE TABLE Venue (
    VenueId INT IDENTITY(1,1) PRIMARY KEY,
    VenueName VARCHAR(100) NOT NULL,
    Location VARCHAR(255),
    Capacity INT,
    ImageUrl VARCHAR(255)
);

-- Create the Event table
CREATE TABLE Event (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName VARCHAR(100) NOT NULL,
    EventDate DATE,
    Description TEXT,
    VenueId INT,
    FOREIGN KEY (VenueId) REFERENCES Venue(VenueId)
);

-- Create the Booking table (associative table)
CREATE TABLE Booking (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT,
    VenueId INT,
    BookingDate DATE,
    FOREIGN KEY (EventId) REFERENCES Event(EventId),
    FOREIGN KEY (VenueId) REFERENCES Venue(VenueId)
);

INSERT INTO Venue (VenueName, Location, Capacity, ImageUrl) VALUES
('Grand Hall', '123 Main St, Cityville', 500, 'https://example.com/images/grandhall.jpg'),
('Oceanview Arena', '456 Beach Rd, Seaside', 1000, 'https://example.com/images/oceanview.jpg'),
('Mountain Lodge', '789 Hilltop Ln, Highland', 300, 'https://example.com/images/lodge.jpg');