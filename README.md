# Gestionarea Farmaciei InfoWorld


## Cerințe:

În cadrul farmaciei, se vor putea face următoarele operațiuni:

- adăugarea, modificarea sau ștergerea pacienților în/din baza de date (nume, prenume, cnp, nr. telefon, adresa, etc);
- doar anumite detalii vor fi obligatorii (la alegerea ta)
- un pacient poate avea una sau mai multe adrese (acasă, muncă etc)
- adăugarea, modificarea sau ștergerea medicamente (denumire, gramaj, forma pastilelor, descriere, lot, data de expirare, nr. bucăți în stoc, etc);
- adăugarea unor comenzi asignate unui pacient din baza de date (se va putea alege orice medicament chiar dacă în momentul respectiv nu va exista stoc în baza de date pentru acel medicament);
- vizualizarea și modificarea pacienților, a medicamentelor și a comenzilor în cadrul unor secțiuni (pagini) dedicate;
- posibilitatea de selectare a mai multor entități (medicamente, pacienți, rețete) și  ștergerea în calup a acestora;
- după crearea unei comenzi de medicamente, vor putea fi disponibile următoarele opțiuni: modificarea, ștergerea sau aprobarea comenzii. La aprobare, se va verifica daca medicamentele din comandă au stoc suficient, iar în caz contrar se va afișa un mesaj corespunzător (fără a finaliza aprobarea). Medicamentele vor fi scăzute din gestiune doar dacă aprobarea a funcționat cu succes. Starea comenzii de a fi aprobată/neaprobată va fi vizibilă și în interfață.


## Suplimentar:

- existența funcționalității de înregistrare utilizator;
- existența funcționalității de autentificare;
- posibilitatea ca după login, sa se facă diferența dintre un farmacist și un pacient.
De exemplu: un pacient nu poate adăuga medicamente noi (doar farmacistul). Un pacient poate crea o comandă (pentru el însuși). Farmacistul va putea adăuga o comandă pentru orice pacient din baza de date. Doar farmacistul va putea aproba o comandă.
- căutarea unui pacient în listă după nume, prenume, cnp.


## Detalii tehnice:

Pentru stocarea, manipularea și salvarea datelor puteți folosi:
- apeluri http catre un server creat local
- baza de date
- fișiere JSON
- variabile locale (in-memory)
- oricare altă metodă cunoscută

## Principalele idei care vor fi urmărite vor fi:

- îndeplinirea a cat mai multor cerințe (de preferat a tuturor);
- implementarea frontend + backend va reprezenta un avantaj;
- implementare de validări (Ex: CNP, nr. de telefon, etc);
- implementarea dinamică / abstractă / generalizată;
- implementarea paginilor într-o manieră responsive (măcar două dimensiuni de ecran);
- folosirea unui sistem de version control (git) va reprezenta un avantaj.
