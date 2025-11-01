# 🎵 MusicCloud

**MusicCloud** ist eine Webapplikation, die im Rahmen der praktischen Arbeit im Sommersemester 2022/23 an der HTL Spengergasse entwickelt wurde.  
Das Projekt wurde mit **ASP.NET Core Razor Pages** in **C#** umgesetzt und erfüllt die Anforderungen an eine CRUD-basierte Webanwendung mit Authentifizierung, Datenbankanbindung und Modellbeziehungen.

---

## 🧑‍💻 Projektbeschreibung

MusicCloud ermöglicht die Verwaltung von **Künstlern (Artists)**, **Alben** und **Songs**.  
Jede Entität ist über CRUD-Funktionalitäten vollständig bearbeitbar (Create, Read, Update, Delete).

Die Anwendung demonstriert den Aufbau einer typischen mehrschichtigen Webapplikation mit Entity Framework, Repositories, DTOs und AutoMapper.

---

## ⚙️ Technologien

- **C# / ASP.NET Core Razor Pages**
- **Entity Framework Core**
- **AutoMapper** für DTO-Mapping
- **SQL Server LocalDB**
- **Bootstrap** für Layout und Styling
- **ASP.NET Identity** für Authentifizierung und Autorisierung

---

## 📁 Projektstruktur

| Ordner | Beschreibung |
|--------|---------------|
| `MusicCloud.Application` | Enthält Models, DTOs, Repositories und AutoMapper-Konfigurationen |
| `MusicCloud.Webapp` | Razor Pages, Controller-Logik und Views |
| `MusicCloud.Test` | (Optional) Testprojekt für Unit-Tests |
| `MusicCloud.sln` | Projekt-Solution-Datei |

---

## 🧩 Features

### CRUD-Operationen
- **Artists:** Erstellen, Anzeigen, Bearbeiten und Löschen von Künstlern  
- **Alben:** Verwaltung von Alben mit Beziehung zu Künstlern  
- **Songs:** Erstellung neuer Songs mit Auswahl des zugehörigen Albums über eine SelectList  

### Detail- und Indexseiten
- Indexseiten zeigen alle Datensätze in Tabellenform mit:
  - Links zu Detail-, Bearbeitungs- und Löschseiten  
  - Anzeige der Anzahl verknüpfter Objekte (z. B. Anzahl der Songs pro Album)

### Validierungen
- Eingabefelder validiert über DataAnnotations  
- SelectLists dynamisch aus der Datenbank geladen  

### Authentifizierung
- Login-Seite mit Benutzerrollen (z. B. Administrator / Benutzer)
- Zugriffsbeschränkung:  
  - Öffentliche Seiten für alle Benutzer  
  - Editier-/Löschfunktionen nur für eingeloggte Benutzer  

---

## 🧠 Erweiterungen gegenüber den Mindestanforderungen

- Verwendung von **mehreren SelectLists** (z. B. Song → Album → Artist)
- **Abfrageoptimierungen** bei der Anzeige der Objektanzahlen
- **Fehlerbehandlung**, sodass keine Exceptions an den Benutzer gelangen
- Saubere **Trennung von Model, DTO und ViewModel** mittels AutoMapper
