CREATE DATABASE IF NOT EXISTS GameDB;
USE GameDB;

CREATE TABLE platforms (
    platform_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE games (
    game_id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    release_date DATE NOT NULL,
    description VARCHAR(255) NOT NULL,
    developer VARCHAR(255) NOT NULL
);

CREATE TABLE reviews (
    review_id INT AUTO_INCREMENT PRIMARY KEY,
    game_id INT NOT NULL,
    date DATETIME NOT NULL,
    rating INT NOT NULL,
    headline VARCHAR(255) NOT NULL,
    text VARCHAR(255) NOT NULL,
    FOREIGN KEY (game_id) REFERENCES games(game_id) ON DELETE CASCADE
);

CREATE TABLE game_platforms (
    game_platform_id INT AUTO_INCREMENT PRIMARY KEY,
    game_id INT NOT NULL,
    platform_id INT NOT NULL,
    FOREIGN KEY (game_id) REFERENCES games(game_id) ON DELETE CASCADE,
    FOREIGN KEY (platform_id) REFERENCES platforms(platform_id) ON DELETE CASCADE,
);


INSERT INTO platforms (name) VALUES
('PC'),
('PlayStation 5'),
('Xbox Series X'),
('Nintendo Switch'),
('PlayStation 4'),
('Xbox One');


INSERT INTO games (title, release_date, description, developer) VALUES
('Cyber Odyssey 2077', '2023-03-15', 'An open-world action RPG set in a dystopian future. Explore Night City and define your legend.', 'CD Future Works'),
('Mystic Realms: Echoes', '2022-11-01', 'A high-fantasy RPG with a rich story and tactical combat. Journey through the land of Eldoria.', 'Dragon Forge Studios'),
('Star Voyager Online', '2024-01-20', 'A massively multiplayer online space exploration and combat game. Chart unknown galaxies.', 'Galaxy Interactive'),
('Pixel Racers Turbo', '2023-07-10', 'A retro-style arcade racing game with vibrant pixel art and a catchy soundtrack.', 'RetroWave Games'),
('Chronicles of Eldoria', '2022-05-12', 'Embark on an epic adventure in a vast open world filled with magic and monsters.', 'Fantasy Softworks');



INSERT INTO reviews (game_id, date, rating, headline, text) VALUES
(1, '2023-03-20 10:00:00', 8, 'A Glimpse into a Dystopian Future', 'Cyber Odyssey 2077 offers a visually stunning world, but has some launch bugs. Great story!'),
(1, '2023-03-22 14:30:00', 9, 'Night City Beckons!', 'Incredible immersion and storytelling. The patches have improved performance significantly.'),
(2, '2022-11-05 09:15:00', 9, 'A Modern Classic RPG', 'Mystic Realms: Echoes delivers on its promise of a deep and engaging RPG experience.'),
(2, '2022-11-10 17:00:00', 7, 'Good, but a bit grindy', 'The story is great, and combat is fun, but some quests feel repetitive. Overall enjoyable.'),
(3, '2024-01-25 11:00:00', 7, 'Vast Universe, Needs More Content', 'Star Voyager Online has a massive galaxy to explore, but currently feels a bit empty.'),
(3, '2024-02-01 16:45:00', 8, 'Promising MMO with Great Potential', 'The core gameplay loop is fun, and the community is growing. Excited for future updates!'),
(4, '2023-07-15 12:00:00', 10, 'Pixel Perfect Fun!', 'Pixel Racers Turbo is pure arcade joy. Amazing soundtrack and tight controls!'),
(5, '2022-06-01 08:00:00', 6, 'Beautiful World, Generic Quests', 'Eldoria is stunning to look at, but the quest design feels uninspired. Decent for a playthrough.');

INSERT INTO game_platforms (game_id, platform_id) VALUES
(1, (SELECT platform_id FROM platforms WHERE name = 'PC')),
(1, (SELECT platform_id FROM platforms WHERE name = 'PlayStation 5')),
(1, (SELECT platform_id FROM platforms WHERE name = 'Xbox Series X'));

INSERT INTO game_platforms (game_id, platform_id) VALUES
(2, (SELECT platform_id FROM platforms WHERE name = 'PC')),
(2, (SELECT platform_id FROM platforms WHERE name = 'PlayStation 4'));

INSERT INTO game_platforms (game_id, platform_id) VALUES
(3, (SELECT platform_id FROM platforms WHERE name = 'PC'));

INSERT INTO game_platforms (game_id, platform_id) VALUES
(4, (SELECT platform_id FROM platforms WHERE name = 'PC')),
(4, (SELECT platform_id FROM platforms WHERE name = 'Nintendo Switch'));

INSERT INTO game_platforms (game_id, platform_id) VALUES
(5, (SELECT platform_id FROM platforms WHERE name = 'PC')),
(5, (SELECT platform_id FROM platforms WHERE name = 'PlayStation 5')),
(5, (SELECT platform_id FROM platforms WHERE name = 'Xbox Series X'));
