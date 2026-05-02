const translations = {
  en: {
    "shell-subtitle": "Country content editor",
    "toggle-theme": "Dark mode",
    "auth-write-title": "Write access enabled",
    "auth-write-copy": "You can add, edit, and delete content.",
    "auth-readonly-title": "Read-only mode",
    "auth-readonly-copy": "Browsing is public. Sign in to edit the database.",
    "auth-unconfigured-title": "Read-only mode",
    "auth-unconfigured-copy": "Admin credentials are not configured yet, so editing is disabled.",
    "auth-login": "Enable write access",
    "auth-logout": "Disable write access",
    "auth-login-title": "Enable write access",
    "auth-username": "Username",
    "auth-password": "Password",
    "auth-login-submit": "Sign in",
    "countries-kicker": "Countries",
    "countries-title": "Available tags",
    "countries-empty": "No countries yet. Add your first tag to begin.",
    "content-label-inline": "entries",
    "content-kicker": "Country content",
    "content-title-empty": "Pick a country",
    "add-content": "Add content",
    "select-country-empty": "Create or select a country to manage its unique content.",
    "content-empty": "This country has no unique content yet.",
    "type-advance": "Advance",
    "type-reform": "Reform",
    "type-default-estate-rename": "Default Estate Rename",
    "type-custom-estate": "Custom Estate",
    "type-privilege": "Privilege",
    "type-law": "Law",
    "type-value": "Value",
    "type-building": "Building",
    "type-event": "Event",
    "detail-kicker": "Details",
    "detail-title": "Selected content",
    "edit-content": "Edit",
    "delete-content": "Delete",
    "detail-empty": "Select a content piece to inspect its effect.",
    "country-modal-title": "Add country",
    "country-name": "Country name",
    "country-name-placeholder": "Kingdom of Example",
    "country-tag": "Country tag",
    "country-tag-placeholder": "EXM",
    "cancel": "Cancel",
    "save-country": "Save country",
    "content-modal-title": "Add new content",
    "content-modal-title-edit": "Edit content",
    "content-type": "Content type",
    "advance-name": "Advance name",
    "advance-name-placeholder": "Centralized Arsenal",
    "reform-name": "Reform name",
    "reform-name-placeholder": "Royal Tax Code",
    "custom-estate-name": "Estate name",
    "custom-estate-name-placeholder": "Frontier Clans",
    "privilege-name": "Privilege name",
    "privilege-name-placeholder": "Merchant Charters",
    "law-name": "Law name",
    "law-name-placeholder": "Guild Autonomy Act",
    "building-name": "Building name",
    "building-name-placeholder": "State Granary",
    "event-name": "Event name",
    "event-name-placeholder": "The City Falls",
    "event-name": "Event name",
    "event-name-placeholder": "The City Falls",
    "value-left-label": "Left side",
    "value-left-placeholder": "Spiritual",
    "value-right-label": "Right side",
    "value-right-placeholder": "Humanist",
    "value-left-effects": "Left-side effects",
    "value-right-effects": "Right-side effects",
    "major-reform": "Major Reform",
    "major-reform-toggle": "Major Reform",
    "advance-modifier-amount": "Modifier amount",
    "advance-modifier-unit": "Value type",
    "unit-flat": "Flat value",
    "unit-percent": "Percentage",
    "save-content": "Save content",
    "save-content-edit": "Save changes",
    "effects-title": "Effects",
    "add-effect": "Add effect",
    "remove-effect": "Remove",
    "effect-label": "Effect label",
    "effect-label-placeholder": "Horse Output",
    "effect-value-type": "Effect kind",
    "effect-kind-numeric": "Numeric",
    "effect-kind-boolean": "Boolean",
    "effect-kind-text": "Custom text",
    "bool-value": "Boolean value",
    "bool-true": "True",
    "bool-false": "False",
    "estate-nobility": "Nobility",
    "estate-burghers": "Burghers",
    "estate-clergy": "Clergy",
    "estate-peasants": "Peasants",
    "estate-nobility-placeholder": "Swordlords",
    "estate-burghers-placeholder": "Guildmasters",
    "estate-clergy-placeholder": "Temple Wardens",
    "estate-peasants-placeholder": "Freeholders",
    "food-consumption": "Food Consumption (per 1000 pops)",
    "assimilation-conversion-speed": "Assimilation and conversion speed (0-100%)",
    "estate-class": "Estate Class",
    "estate-class-upper": "Upper Class",
    "estate-class-lower": "Lower Class",
    "can-promote": "Can Promote?",
    "promotion-speed": "Promotion Speed (%)",
    "migration-speed": "Migration Speed (%)",
    "privilege-estate": "Estate",
    "privilege-estate-nobles": "Nobles",
    "privilege-estate-burghers": "Burghers",
    "privilege-estate-peasants": "Peasants",
    "privilege-estate-clergy": "Clergy",
    "privilege-estate-tribes": "Tribes",
    "privilege-estate-custom": "Custom Estate",
    "privilege-custom-estate": "Custom Estate",
    "privilege-custom-estate-placeholder": "Select custom estate",
    "privilege-satisfaction-bonus": "Satisfaction Bonus (%)",
    "privilege-estate-power": "Estate Power (%)",
    "law-category": "Category",
    "law-category-custom": "Custom Category",
    "law-custom-category": "Custom Category",
    "law-custom-category-placeholder": "Commercial Traditions",
    "law-subcategory": "Subcategory",
    "law-subcategory-custom": "Custom Subcategory",
    "law-custom-subcategory": "Custom Subcategory",
    "law-custom-subcategory-placeholder": "Harbor Ordinances",
    "law-estate-preference": "Preferred Estate",
    "law-custom-estate": "Custom Estate",
    "law-custom-estate-placeholder": "Select custom estate",
    "building-scope": "Construction requirement",
    "building-scope-all": "All locations",
    "building-scope-rural": "Rural only",
    "building-scope-town": "Town only",
    "building-scope-capital": "Capital only",
    "building-ducat-cost": "Build cost (ducats)",
    "building-time-months": "Build time (months)",
    "building-construction-costs": "Construction resources",
    "building-production-methods": "Production methods",
    "add-resource-cost": "Add resource",
    "resource-name": "Resource",
    "resource-name-placeholder": "Wood",
    "resource-amount": "Amount",
    "remove-resource": "Remove",
    "add-production-method": "Add method",
    "remove-production-method": "Remove method",
    "production-method-name": "Production method",
    "production-method-name-placeholder": "Water Mill",
    "production-inputs": "Monthly inputs",
    "production-outputs": "Monthly outputs",
    "event-description": "Description / narrative",
    "event-description-placeholder": "The city walls finally give way after months of bombardment.",
    "event-requirements": "Requirements",
    "event-requirements-helper": "Build nested conditions with AND / OR blocks and drag them where you want.",
    "add-requirement": "Add requirement",
    "remove-requirement": "Remove",
    "event-requirement-expression": "Requirement",
    "event-requirement-placeholder": "religion = religion:catholic",
    "requirement-root-operator": "Top-level operator",
    "requirement-operator-and": "AND",
    "requirement-operator-or": "OR",
    "requirement-group-title": "Group",
    "requirement-condition-title": "Condition",
    "add-condition": "Add condition",
    "add-and-group": "Add AND group",
    "add-or-group": "Add OR group",
    "move-requirement": "Move block",
    "remove-block": "Remove block",
    "drop-here": "Drop here",
    "requirement-empty-group": "This block is empty. Add a condition or another group.",
    "event-year-start": "Year start",
    "event-year-end": "Year end",
    "event-trigger-mode": "Trigger mode",
    "event-trigger-monthly": "Monthly chance",
    "event-trigger-instant": "Instant",
    "event-monthly-chance": "Monthly chance (%)",
    "event-prerequisites": "Previous events required",
    "event-prerequisites-empty": "No events available yet.",
    "event-options": "Options",
    "add-option": "Add option",
    "remove-option": "Remove option",
    "event-option-title": "Option",
    "event-option-text": "Option text",
    "event-option-placeholder": "Seize the treasury",
    "event-option-effects": "Option effects",
    "event-no-instant-effect": "No instant effect"
  },
  pl: {
    "shell-subtitle": "Edytor zawartości kraju",
    "toggle-theme": "Tryb ciemny",
    "auth-write-title": "Tryb edycji włączony",
    "auth-write-copy": "Możesz dodawać, edytować i usuwać zawartość.",
    "auth-readonly-title": "Tryb tylko do odczytu",
    "auth-readonly-copy": "Przeglądanie jest publiczne. Zaloguj się, aby edytować bazę.",
    "auth-unconfigured-title": "Tryb tylko do odczytu",
    "auth-unconfigured-copy": "Dane logowania administratora nie są jeszcze skonfigurowane, więc edycja jest wyłączona.",
    "auth-login": "Włącz edycję",
    "auth-logout": "Wyłącz edycję",
    "auth-login-title": "Włącz edycję",
    "auth-username": "Login",
    "auth-password": "Hasło",
    "auth-login-submit": "Zaloguj się",
    "countries-kicker": "Państwa",
    "countries-title": "Dostępne tagi",
    "countries-empty": "Brak państw. Dodaj pierwszy tag, aby zacząć.",
    "content-label-inline": "wpisów",
    "content-kicker": "Zawartość kraju",
    "content-title-empty": "Wybierz państwo",
    "add-content": "Dodaj zawartość",
    "select-country-empty": "Utwórz lub wybierz państwo, aby zarządzać jego unikalną zawartością.",
    "content-empty": "To państwo nie ma jeszcze unikalnej zawartości.",
    "type-advance": "Postęp",
    "type-reform": "Reforma",
    "type-default-estate-rename": "Zmiana nazw podstawowych stanów",
    "type-custom-estate": "Własny stan",
    "type-privilege": "Przywilej",
    "type-law": "Prawo",
    "type-value": "Wartość",
    "type-building": "Budynek",
    "type-event": "Wydarzenie",
    "detail-kicker": "Szczegóły",
    "detail-title": "Wybrana zawartość",
    "edit-content": "Edytuj",
    "delete-content": "Usuń",
    "detail-empty": "Wybierz element zawartości, aby zobaczyć jego efekt.",
    "country-modal-title": "Dodaj państwo",
    "country-name": "Nazwa państwa",
    "country-name-placeholder": "Królestwo Przykładu",
    "country-tag": "Tag państwa",
    "country-tag-placeholder": "PRZ",
    "cancel": "Anuluj",
    "save-country": "Zapisz państwo",
    "content-modal-title": "Dodaj nową zawartość",
    "content-modal-title-edit": "Edytuj zawartość",
    "content-type": "Typ zawartości",
    "advance-name": "Nazwa postępu",
    "advance-name-placeholder": "Scentralizowany arsenał",
    "reform-name": "Nazwa reformy",
    "reform-name-placeholder": "Królewski kodeks podatkowy",
    "custom-estate-name": "Nazwa stanu",
    "custom-estate-name-placeholder": "Klany pogranicza",
    "privilege-name": "Nazwa przywileju",
    "privilege-name-placeholder": "Karty kupieckie",
    "law-name": "Nazwa prawa",
    "law-name-placeholder": "Akt autonomii cechów",
    "building-name": "Nazwa budynku",
    "event-name": "Nazwa wydarzenia",
    "event-name-placeholder": "Miasto upada",
    "building-name-placeholder": "Spichlerz państwowy",
    "value-left-label": "Lewa strona",
    "value-left-placeholder": "Duchowy",
    "value-right-label": "Prawa strona",
    "value-right-placeholder": "Humanistyczny",
    "value-left-effects": "Efekty lewej strony",
    "value-right-effects": "Efekty prawej strony",
    "major-reform": "Wielka reforma",
    "major-reform-toggle": "To jest wielka reforma",
    "advance-modifier-amount": "Wartość modyfikatora",
    "advance-modifier-unit": "Typ wartości",
    "unit-flat": "Wartość stała",
    "unit-percent": "Procent",
    "save-content": "Zapisz zawartość",
    "save-content-edit": "Zapisz zmiany",
    "effects-title": "Efekty",
    "add-effect": "Dodaj efekt",
    "remove-effect": "Usuń",
    "effect-label": "Opis efektu",
    "effect-label-placeholder": "Wydajność hodowli koni",
    "effect-value-type": "Rodzaj efektu",
    "effect-kind-numeric": "Liczbowy",
    "effect-kind-boolean": "Logiczny",
    "effect-kind-text": "Tekst własny",
    "bool-value": "Wartość logiczna",
    "bool-true": "Prawda",
    "bool-false": "Fałsz",
    "estate-nobility": "Szlachta",
    "estate-burghers": "Mieszczanie",
    "estate-clergy": "Duchowieństwo",
    "estate-peasants": "Chłopi",
    "estate-nobility-placeholder": "Mieczowi panowie",
    "estate-burghers-placeholder": "Mistrzowie cechów",
    "estate-clergy-placeholder": "Strażnicy świątyń",
    "estate-peasants-placeholder": "Wolni gospodarze",
    "food-consumption": "Zużycie żywności (na 1000 populacji)",
    "assimilation-conversion-speed": "Szybkość asymilacji i konwersji (0-100%)",
    "estate-class": "Klasa stanu",
    "estate-class-upper": "Wyższa klasa",
    "estate-class-lower": "Niższa klasa",
    "can-promote": "Może awansować?",
    "promotion-speed": "Szybkość awansu (%)",
    "migration-speed": "Szybkość migracji (%)",
    "privilege-estate": "Stan",
    "privilege-estate-nobles": "Szlachta",
    "privilege-estate-burghers": "Mieszczanie",
    "privilege-estate-peasants": "Chłopi",
    "privilege-estate-clergy": "Duchowieństwo",
    "privilege-estate-tribes": "Plemiona",
    "privilege-estate-custom": "Własny stan",
    "privilege-custom-estate": "Własny stan",
    "privilege-custom-estate-placeholder": "Wybierz własny stan",
    "privilege-satisfaction-bonus": "Premia do zadowolenia (%)",
    "privilege-estate-power": "Siła stanu (%)",
    "law-category": "Kategoria",
    "law-category-custom": "Własna kategoria",
    "law-custom-category": "Własna kategoria",
    "law-custom-category-placeholder": "Tradycje handlowe",
    "law-subcategory": "Podkategoria",
    "law-subcategory-custom": "Własna podkategoria",
    "law-custom-subcategory": "Własna podkategoria",
    "law-custom-subcategory-placeholder": "Ordynacje portowe",
    "law-estate-preference": "Preferowany stan",
    "law-custom-estate": "Własny stan",
    "law-custom-estate-placeholder": "Wybierz własny stan",
    "building-scope": "Wymóg budowy",
    "building-scope-all": "Wszystkie lokacje",
    "building-scope-rural": "Tylko wieś",
    "building-scope-town": "Tylko miasto",
    "building-scope-capital": "Tylko stolica",
    "building-ducat-cost": "Koszt budowy (dukaty)",
    "building-time-months": "Czas budowy (miesiące)",
    "building-construction-costs": "Surowce do budowy",
    "building-production-methods": "Metody produkcji",
    "add-resource-cost": "Dodaj surowiec",
    "resource-name": "Surowiec",
    "resource-name-placeholder": "Drewno",
    "resource-amount": "Ilość",
    "remove-resource": "Usuń",
    "add-production-method": "Dodaj metodę",
    "remove-production-method": "Usuń metodę",
    "production-method-name": "Metoda produkcji",
    "production-method-name-placeholder": "Młyn wodny",
    "production-inputs": "Miesięczne wejścia",
    "production-outputs": "Miesięczne wyjścia"
  }
};

const lawCatalog = {
  Religious: ["Iqṭā'","Inquisition Law","Society of Jesus","Role of the Patriarchate","Censorship","Slave Conversion","Holy Mission","Nature of our Faith","Divine Cause","Tribal Religious Values","Marriage Law","Heir Religion","Reformation Edicts","Status of Jansenism","Authority over the Clergy","The Witchcraft Act","The Holy Office","Monastic Reformation Movement"],
  Military: ["Levy Law","Recruitment Law","Battle Leadership Law","Army Doctrine","Mamlūk Rank System","Mamlūk Army Law","Mamlūk Promotion Law","Maritime Law","Piracy Law","Naval Doctrine","Sumptuary Law","Order of Chivalry","The Frontier Fortresses","Military Frontier"],
  Administrative: ["Feudal 'De Jure' Laws","Royal Court Customs","Legitimization of Power","Harem Laws","Foundation of Republic","Election","Republican Power","Administrative Principles","Republican Electorate Law","Theocratic Leadership","Legal Basis","Tribal Organization","Administrative System","Intelligence Agency","Bureaucracy","Cabinet Laws","Legal Code","Sharī'ah Jurisprudence","Order of the Garter","Machault's Five Percent Tax","Navarrese Adaptability","Six Ministries Supervision","Ashta Pradhan Council","Mesta Council","Examination System","Status of the Han","Distribution of Scottish Clan Holdings","Taxation","Dahsala Taxation"],
  Socioeconomic: ["Tribal Migration","Tribal Cultural Identity","Tribal Modernization","Education of the Elites","Education of the Masses","Slavery Laws","Colonial Policy","Native Policy","Economic Policy","Cultural Traditions","Foreign Cultural Law","Coinage Law","Precious Metal Distribution","Press Laws","Mining Law","Amazigh Familial Connections","Border Controls","Language of Pleading","Colonial Aspirations","Primacy of Florentine Guilds","Estudo Geral","Amber Monopoly","Dominant Currency","Scottish Colonial Affairs","Enclosure Movement","Factory Working Conditions","Trade Unions","Vestiarium","Tìfāyìfú","Intellectual Movements","Alcohol Restrictions"],
  Estate: ["Fouage Tax Levy","Taille","Distribution of Power","Rights of the Nobility","Rights of the Burghers","Rights of the Clergy","Rights of the Crown","Rights of the Commoners","Colonial Representation"]
};

const body = document.body;
const themeToggle = document.querySelector("[data-theme-toggle]");
const languageButtons = document.querySelectorAll("[data-language]");
const addContentButton = document.querySelector("[data-add-content-button]");
const editContentButton = document.querySelector("[data-edit-content-button]");
const contentForm = document.querySelector("#contentForm");
const typeSelect = document.querySelector("[data-content-type-select]");
const contentIdInput = document.querySelector("[data-content-id-input]");
const contentModalTitle = document.querySelector("[data-content-modal-title]");
const submitContentButton = document.querySelector("[data-submit-content-button]");
const selectedContentPayloadElement = document.querySelector("#selected-content-payload");
const effectsPanel = document.querySelector("[data-effects-panel]");
const estateRenamePanel = document.querySelector("[data-estate-rename-panel]");
const customEstatePanel = document.querySelector("[data-custom-estate-panel]");
const privilegePanel = document.querySelector("[data-privilege-panel]");
const lawPanel = document.querySelector("[data-law-panel]");
const valuePanel = document.querySelector("[data-value-panel]");
const buildingPanel = document.querySelector("[data-building-panel]");
const eventPanel = document.querySelector("[data-event-panel]");
const reformOptions = document.querySelector("[data-reform-options]");
const effectsContainer = document.querySelector("[data-effects-container]");
const leftEffectsContainer = document.querySelector("[data-left-effects-container]");
const rightEffectsContainer = document.querySelector("[data-right-effects-container]");
const constructionCostsContainer = document.querySelector("[data-construction-costs-container]");
const productionMethodsContainer = document.querySelector("[data-production-methods-container]");
const eventRequirementTreeInput = document.querySelector("[data-event-requirement-tree-input]");
const eventRequirementBuilder = document.querySelector("[data-event-requirement-builder]");
const eventOptionsContainer = document.querySelector("[data-event-options-container]");
const addEffectButton = document.querySelector("[data-add-effect]");
const addLeftEffectButton = document.querySelector("[data-add-left-effect]");
const addRightEffectButton = document.querySelector("[data-add-right-effect]");
const addConstructionCostButton = document.querySelector("[data-add-construction-cost]");
const addProductionMethodButton = document.querySelector("[data-add-production-method]");
const addEventOptionButton = document.querySelector("[data-add-event-option]");
const effectTemplate = document.querySelector("#effect-row-template");
const leftEffectTemplate = document.querySelector("#left-effect-row-template");
const rightEffectTemplate = document.querySelector("#right-effect-row-template");
const constructionCostTemplate = document.querySelector("#construction-cost-row-template");
const productionMethodTemplate = document.querySelector("#production-method-template");
const productionResourceTemplate = document.querySelector("#production-resource-row-template");
const eventOptionTemplate = document.querySelector("#event-option-template");
const eventOptionEffectTemplate = document.querySelector("#event-option-effect-row-template");
const contentNameRow = document.querySelector("[data-content-name-row]");
const contentNameLabel = document.querySelector("[data-content-name-label]");
const contentNameInput = document.querySelector("[data-content-name-input]");
const canPromoteToggle = document.querySelector("[data-can-promote-toggle]");
const promotionSpeedRow = document.querySelector("[data-promotion-speed-row]");
const privilegeEstateSelect = document.querySelector("[data-privilege-estate-select]");
const privilegeCustomEstateRow = document.querySelector("[data-privilege-custom-estate-row]");
const lawCategorySelect = document.querySelector("[data-law-category-select]");
const lawSubcategorySelect = document.querySelector("[data-law-subcategory-select]");
const lawCustomCategoryRow = document.querySelector("[data-law-custom-category-row]");
const lawSubcategoryRow = document.querySelector("[data-law-subcategory-row]");
const lawCustomSubcategoryRow = document.querySelector("[data-law-custom-subcategory-row]");
const lawEstateSelect = document.querySelector("[data-law-estate-select]");
const lawCustomEstateRow = document.querySelector("[data-law-custom-estate-row]");
const eventTriggerModeSelect = document.querySelector("[data-event-trigger-mode-select]");
const eventMonthlyChanceRow = document.querySelector("[data-event-monthly-chance-row]");
const selectedContentData = selectedContentPayloadElement ? JSON.parse(selectedContentPayloadElement.textContent) : null;
let eventRequirementTreeState = null;
let draggedRequirementNodeId = null;

const contentNameKeys = {
  Advance: { label: "advance-name", placeholder: "advance-name-placeholder" },
  Reform: { label: "reform-name", placeholder: "reform-name-placeholder" },
  CustomEstate: { label: "custom-estate-name", placeholder: "custom-estate-name-placeholder" },
  Privilege: { label: "privilege-name", placeholder: "privilege-name-placeholder" },
  Law: { label: "law-name", placeholder: "law-name-placeholder" },
  Building: { label: "building-name", placeholder: "building-name-placeholder" },
  Event: { label: "event-name", placeholder: "event-name-placeholder" }
};

function applyLanguage(language) {
  const dictionary = translations[language] || translations.en;
  body.dataset.lang = language;
  document.querySelectorAll("[data-i18n]").forEach((element) => {
    const key = element.dataset.i18n;
    if (dictionary[key]) element.textContent = dictionary[key];
  });
  document.querySelectorAll("[data-i18n-placeholder]").forEach((element) => {
    const key = element.dataset.i18nPlaceholder;
    if (dictionary[key]) element.setAttribute("placeholder", dictionary[key]);
  });
  document.querySelectorAll("[data-i18n-option]").forEach((element) => {
    const key = element.dataset.i18nOption;
    if (dictionary[key]) element.textContent = dictionary[key];
  });
  languageButtons.forEach((button) => button.classList.toggle("is-active", button.dataset.language === language));
  updateModalModeText();
  renderRequirementTree();
  localStorage.setItem("planner-language", language);
}

function applyTheme(theme) {
  body.classList.toggle("theme-dark", theme === "dark");
  body.classList.toggle("theme-light", theme === "light");
  localStorage.setItem("planner-theme", theme);
}

function syncPromotionSpeed() {
  if (!promotionSpeedRow) return;
  const isVisible = typeSelect && typeSelect.value === "CustomEstate" && canPromoteToggle && canPromoteToggle.checked;
  promotionSpeedRow.hidden = !isVisible;
}

function syncEventTriggerFields() {
  if (!eventTriggerModeSelect) return;
  const isInstant = typeSelect && typeSelect.value === "Event" && eventTriggerModeSelect.value === "Instant";
  if (eventMonthlyChanceRow) eventMonthlyChanceRow.hidden = isInstant;
}

function syncPrivilegeCustomEstate() {
  if (!privilegeCustomEstateRow) return;
  const isVisible = typeSelect && typeSelect.value === "Privilege" && privilegeEstateSelect && privilegeEstateSelect.value === "Custom";
  privilegeCustomEstateRow.hidden = !isVisible;
}

function rebuildLawSubcategories() {
  if (!lawCategorySelect || !lawSubcategorySelect) return;
  const currentCategory = lawCategorySelect.value;
  const dictionary = translations[body.dataset.lang || "en"] || translations.en;
  const previousValue = lawSubcategorySelect.value;
  lawSubcategorySelect.innerHTML = "";
  if (currentCategory === "Custom") return;
  const subcategories = lawCatalog[currentCategory] || [];
  subcategories.forEach((subcategory) => {
    const option = document.createElement("option");
    option.value = subcategory;
    option.textContent = subcategory;
    lawSubcategorySelect.appendChild(option);
  });
  const customOption = document.createElement("option");
  customOption.value = "Custom";
  customOption.dataset.i18nOption = "law-subcategory-custom";
  customOption.textContent = dictionary["law-subcategory-custom"] || "Custom Subcategory";
  lawSubcategorySelect.appendChild(customOption);
  const nextValue = subcategories.includes(previousValue) || previousValue === "Custom" ? previousValue : (subcategories[0] || "Custom");
  lawSubcategorySelect.value = nextValue;
}

function syncLawFields() {
  if (!lawPanel) return;
  const categoryIsCustom = lawCategorySelect && lawCategorySelect.value === "Custom";
  const subcategoryIsCustom = lawSubcategorySelect && lawSubcategorySelect.value === "Custom";
  const lawTypeSelected = typeSelect && typeSelect.value === "Law";
  const estateIsCustom = lawEstateSelect && lawEstateSelect.value === "Custom";
  if (lawCustomCategoryRow) lawCustomCategoryRow.hidden = !(lawTypeSelected && categoryIsCustom);
  if (lawSubcategoryRow) lawSubcategoryRow.hidden = !!(lawTypeSelected && categoryIsCustom);
  if (lawCustomSubcategoryRow) lawCustomSubcategoryRow.hidden = !(lawTypeSelected && (categoryIsCustom || subcategoryIsCustom));
  if (lawCustomEstateRow) lawCustomEstateRow.hidden = !(lawTypeSelected && estateIsCustom);
}

function syncContentTypePanel() {
  const currentType = typeSelect ? typeSelect.value : "Advance";
  if (effectsPanel) effectsPanel.hidden = !["Advance", "Reform", "Privilege", "Law", "Building"].includes(currentType);
  if (estateRenamePanel) estateRenamePanel.hidden = currentType !== "DefaultEstateRename";
  if (customEstatePanel) customEstatePanel.hidden = currentType !== "CustomEstate";
  if (privilegePanel) privilegePanel.hidden = currentType !== "Privilege";
  if (lawPanel) lawPanel.hidden = currentType !== "Law";
  if (valuePanel) valuePanel.hidden = currentType !== "Value";
  if (buildingPanel) buildingPanel.hidden = currentType !== "Building";
  if (eventPanel) eventPanel.hidden = currentType !== "Event";
  if (reformOptions) reformOptions.hidden = currentType !== "Reform";
  if (contentNameRow) contentNameRow.hidden = currentType === "DefaultEstateRename" || currentType === "Value";

  const keys = contentNameKeys[currentType] || contentNameKeys.Advance;
  const dictionary = translations[body.dataset.lang || "en"] || translations.en;
  if (contentNameRow && !contentNameRow.hidden && contentNameLabel && keys) {
    contentNameLabel.dataset.i18n = keys.label;
    contentNameLabel.textContent = dictionary[keys.label];
  }
  if (contentNameRow && !contentNameRow.hidden && contentNameInput && keys) {
    contentNameInput.dataset.i18nPlaceholder = keys.placeholder;
    contentNameInput.setAttribute("placeholder", dictionary[keys.placeholder]);
  }
  syncPromotionSpeed();
  syncEventTriggerFields();
  syncPrivilegeCustomEstate();
  rebuildLawSubcategories();
  syncLawFields();
}

function syncEffectRow(row) {
  const typeSelectInRow = row.querySelector("[data-effect-value-type]");
  const valueType = typeSelectInRow ? typeSelectInRow.value : "Numeric";
  const isBoolean = valueType === "Boolean";
  const isText = valueType === "Text";
  row.querySelectorAll("[data-effect-numeric]").forEach((element) => { element.hidden = isBoolean || isText; });
  row.querySelectorAll("[data-effect-boolean]").forEach((element) => { element.hidden = !isBoolean; });
}

function bindEffectRow(row, container, groupName) {
  const typeSelectInRow = row.querySelector("[data-effect-value-type]");
  if (typeSelectInRow) typeSelectInRow.addEventListener("change", () => syncEffectRow(row));
  const removeButton = row.querySelector("[data-remove-effect]");
  if (removeButton) {
    removeButton.addEventListener("click", () => {
      if (container.querySelectorAll("[data-effect-row]").length > 1) {
        row.remove();
        reindexEffectRows(container, groupName);
      }
    });
  }
  syncEffectRow(row);
}

function reindexEffectRows(container, groupName) {
  if (!container) return;
  container.querySelectorAll("[data-effect-row]").forEach((row, index) => {
    row.querySelectorAll("[name]").forEach((field) => {
      field.name = field.name.replace(new RegExp(`AdvanceForm\\.${groupName}\\[\\d+\\]`), `AdvanceForm.${groupName}[${index}]`);
    });
  });
}

function addEffectRow(container, template, groupName) {
  if (!template || !container) return null;
  const currentIndex = container.querySelectorAll("[data-effect-row]").length;
  container.insertAdjacentHTML("beforeend", template.innerHTML.replaceAll("__INDEX__", String(currentIndex)));
  const newRow = container.querySelectorAll("[data-effect-row]")[currentIndex];
  bindEffectRow(newRow, container, groupName);
  applyLanguage(body.dataset.lang || "en");
  return newRow;
}

function defaultEffect() {
  return { label: "", valueType: "Numeric", numericAmount: 0, numericUnit: "Flat", boolValue: false };
}

function setNamedFieldValue(name, value) {
  const field = contentForm ? contentForm.querySelector(`[name="${cssEscape(name)}"]`) : null;
  if (!field) return;
  if (field.type === "checkbox") {
    field.checked = Boolean(value);
    return;
  }
  field.value = value ?? "";
}

function setEffectRowValues(row, groupName, index, effect) {
  const base = `AdvanceForm.${groupName}[${index}]`;
  setNamedFieldValue(`${base}.Label`, effect.label ?? "");
  setNamedFieldValue(`${base}.ValueType`, effect.valueType ?? "Numeric");
  setNamedFieldValue(`${base}.NumericAmount`, effect.numericAmount ?? 0);
  setNamedFieldValue(`${base}.NumericUnit`, effect.numericUnit ?? "Flat");
  setNamedFieldValue(`${base}.BoolValue`, String(effect.boolValue ?? false));
  syncEffectRow(row);
}

function buildEffectRows(container, template, groupName, effects) {
  if (!container || !template) return;
  container.innerHTML = "";
  const items = effects && effects.length > 0 ? effects : [defaultEffect()];
  items.forEach((effect, index) => {
    const row = addEffectRow(container, template, groupName);
    if (row) setEffectRowValues(row, groupName, index, effect);
  });
  reindexEffectRows(container, groupName);
}

function bindResourceRow(row, container, reindexFn) {
  const removeButton = row.querySelector("[data-remove-resource]");
  if (removeButton) {
    removeButton.addEventListener("click", () => {
      row.remove();
      reindexFn();
    });
  }
}

function addConstructionCostRow(cost = { resourceName: "", amount: 0 }) {
  if (!constructionCostsContainer || !constructionCostTemplate) return;
  const index = constructionCostsContainer.querySelectorAll("[data-resource-row]").length;
  constructionCostsContainer.insertAdjacentHTML("beforeend", constructionCostTemplate.innerHTML.replaceAll("__INDEX__", String(index)));
  const row = constructionCostsContainer.querySelectorAll("[data-resource-row]")[index];
  row.querySelector(`[name="AdvanceForm.ConstructionCosts[${index}].ResourceName"]`).value = cost.resourceName ?? "";
  row.querySelector(`[name="AdvanceForm.ConstructionCosts[${index}].Amount"]`).value = cost.amount ?? 0;
  bindResourceRow(row, constructionCostsContainer, reindexConstructionCosts);
  applyLanguage(body.dataset.lang || "en");
}

function reindexConstructionCosts() {
  if (!constructionCostsContainer) return;
  constructionCostsContainer.querySelectorAll("[data-resource-row]").forEach((row, index) => {
    row.querySelectorAll("[name]").forEach((field) => {
      field.name = field.name.replace(/AdvanceForm\.ConstructionCosts\[\d+\]/, `AdvanceForm.ConstructionCosts[${index}]`);
    });
  });
}

function createProductionResourceRowHtml(methodIndex, kind, resourceIndex) {
  return productionResourceTemplate.innerHTML
    .replace("__NAME__", `AdvanceForm.ProductionMethods[${methodIndex}].${kind}[${resourceIndex}].ResourceName`)
    .replace("__AMOUNT_NAME__", `AdvanceForm.ProductionMethods[${methodIndex}].${kind}[${resourceIndex}].Amount`);
}

function bindProductionMethodRow(row) {
  const addInputButton = row.querySelector("[data-add-production-input]");
  const addOutputButton = row.querySelector("[data-add-production-output]");
  const removeMethodButton = row.querySelector("[data-remove-production-method]");
  const inputsContainer = row.querySelector("[data-production-inputs-container]");
  const outputsContainer = row.querySelector("[data-production-outputs-container]");

  if (addInputButton) {
    addInputButton.addEventListener("click", () => {
      const methodIndex = Array.from(productionMethodsContainer.querySelectorAll("[data-production-method-row]")).indexOf(row);
      const resourceIndex = inputsContainer.querySelectorAll("[data-resource-row]").length;
      inputsContainer.insertAdjacentHTML("beforeend", createProductionResourceRowHtml(methodIndex, "Inputs", resourceIndex));
      const newRow = inputsContainer.querySelectorAll("[data-resource-row]")[resourceIndex];
      bindResourceRow(newRow, inputsContainer, reindexProductionMethods);
      applyLanguage(body.dataset.lang || "en");
      reindexProductionMethods();
    });
  }

  if (addOutputButton) {
    addOutputButton.addEventListener("click", () => {
      const methodIndex = Array.from(productionMethodsContainer.querySelectorAll("[data-production-method-row]")).indexOf(row);
      const resourceIndex = outputsContainer.querySelectorAll("[data-resource-row]").length;
      outputsContainer.insertAdjacentHTML("beforeend", createProductionResourceRowHtml(methodIndex, "Outputs", resourceIndex));
      const newRow = outputsContainer.querySelectorAll("[data-resource-row]")[resourceIndex];
      bindResourceRow(newRow, outputsContainer, reindexProductionMethods);
      applyLanguage(body.dataset.lang || "en");
      reindexProductionMethods();
    });
  }

  if (removeMethodButton) {
    removeMethodButton.addEventListener("click", () => {
      row.remove();
      reindexProductionMethods();
    });
  }

  row.querySelectorAll("[data-production-inputs-container] [data-resource-row], [data-production-outputs-container] [data-resource-row]").forEach((resourceRow) => {
    const container = resourceRow.parentElement;
    bindResourceRow(resourceRow, container, reindexProductionMethods);
  });
}

function reindexProductionMethods() {
  if (!productionMethodsContainer) return;
  productionMethodsContainer.querySelectorAll("[data-production-method-row]").forEach((row, methodIndex) => {
    row.querySelectorAll("[name]").forEach((field) => {
      field.name = field.name
        .replace(/AdvanceForm\.ProductionMethods\[\d+\]/, `AdvanceForm.ProductionMethods[${methodIndex}]`)
        .replace(/\.Inputs\[\d+\]/, (match) => {
          const resourceRow = field.closest("[data-resource-row]");
          const container = resourceRow ? resourceRow.parentElement : null;
          const resourceIndex = container ? Array.from(container.querySelectorAll("[data-resource-row]")).indexOf(resourceRow) : 0;
          return `.Inputs[${resourceIndex}]`;
        })
        .replace(/\.Outputs\[\d+\]/, (match) => {
          const resourceRow = field.closest("[data-resource-row]");
          const container = resourceRow ? resourceRow.parentElement : null;
          const resourceIndex = container ? Array.from(container.querySelectorAll("[data-resource-row]")).indexOf(resourceRow) : 0;
          return `.Outputs[${resourceIndex}]`;
        });
    });
  });
}

function addProductionMethodRow(method = null) {
  if (!productionMethodsContainer || !productionMethodTemplate) return;
  const methodIndex = productionMethodsContainer.querySelectorAll("[data-production-method-row]").length;
  productionMethodsContainer.insertAdjacentHTML("beforeend", productionMethodTemplate.innerHTML.replaceAll("__METHOD_INDEX__", String(methodIndex)));
  const row = productionMethodsContainer.querySelectorAll("[data-production-method-row]")[methodIndex];
  bindProductionMethodRow(row);
  if (method) {
    row.querySelector(`[name="AdvanceForm.ProductionMethods[${methodIndex}].Name"]`).value = method.name ?? "";
    const inputsContainer = row.querySelector("[data-production-inputs-container]");
    const outputsContainer = row.querySelector("[data-production-outputs-container]");
    inputsContainer.innerHTML = "";
    outputsContainer.innerHTML = "";
    (method.inputs && method.inputs.length > 0 ? method.inputs : [{ resourceName: "", amount: 0 }]).forEach((input, inputIndex) => {
      inputsContainer.insertAdjacentHTML("beforeend", createProductionResourceRowHtml(methodIndex, "Inputs", inputIndex));
      const inputRow = inputsContainer.querySelectorAll("[data-resource-row]")[inputIndex];
      inputRow.querySelector(`[name="AdvanceForm.ProductionMethods[${methodIndex}].Inputs[${inputIndex}].ResourceName"]`).value = input.resourceName ?? "";
      inputRow.querySelector(`[name="AdvanceForm.ProductionMethods[${methodIndex}].Inputs[${inputIndex}].Amount"]`).value = input.amount ?? 0;
      bindResourceRow(inputRow, inputsContainer, reindexProductionMethods);
    });
    (method.outputs && method.outputs.length > 0 ? method.outputs : [{ resourceName: "", amount: 0 }]).forEach((output, outputIndex) => {
      outputsContainer.insertAdjacentHTML("beforeend", createProductionResourceRowHtml(methodIndex, "Outputs", outputIndex));
      const outputRow = outputsContainer.querySelectorAll("[data-resource-row]")[outputIndex];
      outputRow.querySelector(`[name="AdvanceForm.ProductionMethods[${methodIndex}].Outputs[${outputIndex}].ResourceName"]`).value = output.resourceName ?? "";
      outputRow.querySelector(`[name="AdvanceForm.ProductionMethods[${methodIndex}].Outputs[${outputIndex}].Amount"]`).value = output.amount ?? 0;
      bindResourceRow(outputRow, outputsContainer, reindexProductionMethods);
    });
  }
  applyLanguage(body.dataset.lang || "en");
  reindexProductionMethods();
}

function buildConstructionCostRows(costs) {
  if (!constructionCostsContainer) return;
  constructionCostsContainer.innerHTML = "";
  const items = costs && costs.length > 0 ? costs : [{ resourceName: "", amount: 0 }];
  items.forEach((cost) => addConstructionCostRow(cost));
  reindexConstructionCosts();
}

function buildProductionMethodRows(methods) {
  if (!productionMethodsContainer) return;
  productionMethodsContainer.innerHTML = "";
  const items = methods && methods.length > 0 ? methods : [{ name: "", inputs: [{ resourceName: "", amount: 0 }], outputs: [{ resourceName: "", amount: 0 }] }];
  items.forEach((method) => addProductionMethodRow(method));
  reindexProductionMethods();
}

function generateRequirementNodeId() {
  return `req-${Math.random().toString(36).slice(2, 10)}${Date.now().toString(36)}`;
}

function createRequirementCondition(expression = "") {
  return {
    id: generateRequirementNodeId(),
    nodeType: "Condition",
    groupOperator: "And",
    expression,
    children: []
  };
}

function createRequirementGroup(groupOperator = "And", children = []) {
  return {
    id: generateRequirementNodeId(),
    nodeType: "Group",
    groupOperator,
    expression: "",
    children
  };
}

function normalizeRequirementNode(node) {
  if (!node || typeof node !== "object") {
    return createRequirementGroup("And", [createRequirementCondition("")]);
  }

  const normalizedType = node.nodeType === "Group" || node.nodeType === 1 ? "Group" : "Condition";
  if (normalizedType === "Group") {
    const operator = node.groupOperator === "Or" || node.groupOperator === 1 ? "Or" : "And";
    const children = Array.isArray(node.children) ? node.children.map((child) => normalizeRequirementNode(child)) : [];
    return {
      id: node.id || generateRequirementNodeId(),
      nodeType: "Group",
      groupOperator: operator,
      expression: "",
      children
    };
  }

  return {
    id: node.id || generateRequirementNodeId(),
    nodeType: "Condition",
    groupOperator: "And",
    expression: node.expression || "",
    children: []
  };
}

function normalizeRequirementTree(tree) {
  if (Array.isArray(tree)) {
    const legacyChildren = tree
      .map((item) => {
        if (typeof item === "string") return createRequirementCondition(item);
        if (item && typeof item === "object" && typeof item.expression === "string") return createRequirementCondition(item.expression);
        return null;
      })
      .filter(Boolean);
    return createRequirementGroup("And", legacyChildren.length > 0 ? legacyChildren : [createRequirementCondition("")]);
  }

  const normalized = normalizeRequirementNode(tree);
  if (normalized.nodeType !== "Group") {
    return createRequirementGroup("And", [normalized]);
  }

  if (!Array.isArray(normalized.children) || normalized.children.length === 0) {
    normalized.children = [createRequirementCondition("")];
  }

  return normalized;
}

function requirementDictionary() {
  return translations[body.dataset.lang || "en"] || translations.en;
}

function escapeHtml(value) {
  return String(value ?? "")
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    .replaceAll("\"", "&quot;");
}

function serializeRequirementTree() {
  if (!eventRequirementTreeInput || !eventRequirementTreeState) return;
  eventRequirementTreeInput.value = JSON.stringify(eventRequirementTreeState);
}

function renderRequirementDropZone(groupId, index) {
  const dictionary = requirementDictionary();
  return `<div class="requirement-drop-zone" data-req-drop-zone data-target-group-id="${groupId}" data-target-index="${index}" aria-label="${escapeHtml(dictionary["drop-here"] || "Drop here")}"></div>`;
}

function renderRequirementNode(node, isRoot = false) {
  const dictionary = requirementDictionary();
  const operatorText = node.groupOperator === "Or"
    ? (dictionary["requirement-operator-or"] || "OR")
    : (dictionary["requirement-operator-and"] || "AND");
  const operatorSummary = node.groupOperator === "Or"
    ? (dictionary["requirement-summary-or"] || "Any of these can be true")
    : (dictionary["requirement-summary-and"] || "All of these must be true");

  if (node.nodeType === "Group") {
    const childMarkup = node.children.length === 0
      ? `<div class="requirement-empty-state">${escapeHtml(dictionary["requirement-empty-group"] || "This block is empty. Add a condition or another group.")}</div>`
      : `${renderRequirementDropZone(node.id, 0)}${node.children.map((child, index) => `${renderRequirementNode(child)}${renderRequirementDropZone(node.id, index + 1)}`).join("")}`;

    return `
      <div class="requirement-group-card requirement-node" data-req-node-id="${node.id}" data-root-group="${isRoot}" ${isRoot ? "" : 'draggable="true"'}>
        <div class="requirement-card-header">
          <div class="requirement-card-title">
            ${isRoot ? "" : `<span class="requirement-drag-handle" title="${escapeHtml(dictionary["move-requirement"] || "Move block")}">::</span>`}
            <span class="requirement-kicker">${escapeHtml(isRoot ? (dictionary["requirement-root-title"] || "Requirement logic") : (dictionary["requirement-group-title"] || "Group"))}</span>
            <strong class="requirement-summary-text">${escapeHtml(operatorSummary)}</strong>
          </div>
          <div class="requirement-inline-controls requirement-inline-controls-operator">
            <div class="segmented-control requirement-operator-toggle" role="group" aria-label="${escapeHtml(dictionary["requirement-root-operator"] || "Operator")}">
              <button type="button" class="segment ${node.groupOperator === "And" ? "is-active" : ""}" data-req-set-operator="And" data-req-node-id="${node.id}">${escapeHtml(dictionary["requirement-operator-and"] || "AND")}</button>
              <button type="button" class="segment ${node.groupOperator === "Or" ? "is-active" : ""}" data-req-set-operator="Or" data-req-node-id="${node.id}">${escapeHtml(dictionary["requirement-operator-or"] || "OR")}</button>
            </div>
          </div>
        </div>
        ${isRoot ? `<p class="helper-copy requirement-helper">${escapeHtml(dictionary["event-requirements-helper"] || "Build nested conditions with AND / OR blocks and drag them where you want.")}</p>` : ""}
        <div class="requirement-children" data-req-children data-group-id="${node.id}">
          ${childMarkup}
        </div>
        <div class="requirement-card-actions">
          <button type="button" class="material-button ghost-button small-button" data-req-add-condition data-req-node-id="${node.id}">${escapeHtml(dictionary["add-condition"] || "Add condition")}</button>
          <button type="button" class="material-button ghost-button small-button" data-req-add-group="And" data-req-node-id="${node.id}">${escapeHtml(dictionary["add-and-group"] || "Add AND group")}</button>
          <button type="button" class="material-button ghost-button small-button" data-req-add-group="Or" data-req-node-id="${node.id}">${escapeHtml(dictionary["add-or-group"] || "Add OR group")}</button>
          ${isRoot ? "" : `<button type="button" class="material-button danger-button small-button" data-req-remove-node data-req-node-id="${node.id}">${escapeHtml(dictionary["remove-block"] || "Remove block")}</button>`}
        </div>
      </div>`;
  }

  return `
    <div class="requirement-condition-card requirement-node" data-req-node-id="${node.id}" draggable="true">
      <div class="requirement-card-header">
        <div class="requirement-card-title">
          <span class="requirement-drag-handle" title="${escapeHtml(dictionary["move-requirement"] || "Move block")}">::</span>
          <span class="requirement-condition-prefix">${escapeHtml(dictionary["requirement-condition-prefix"] || "IF")}</span>
        </div>
        <button type="button" class="material-button danger-button small-button" data-req-remove-node data-req-node-id="${node.id}">${escapeHtml(dictionary["remove-block"] || "Remove block")}</button>
      </div>
      <input class="form-control requirement-input" data-req-expression data-req-node-id="${node.id}" placeholder="${escapeHtml(dictionary["event-requirement-placeholder"] || "religion = religion:catholic")}" value="${escapeHtml(node.expression || "")}" />
    </div>`;
}

function renderRequirementTree() {
  if (!eventRequirementBuilder || !eventRequirementTreeState) return;
  eventRequirementBuilder.innerHTML = renderRequirementNode(eventRequirementTreeState, true);
  serializeRequirementTree();
}

function buildEventRequirementTree(tree) {
  eventRequirementTreeState = normalizeRequirementTree(tree);
  renderRequirementTree();
}

function findRequirementNodeWithParent(node, id, parent = null) {
  if (!node) return null;
  if (node.id === id) return { node, parent };
  if (!Array.isArray(node.children)) return null;
  for (const child of node.children) {
    const result = findRequirementNodeWithParent(child, id, node);
    if (result) return result;
  }
  return null;
}

function removeRequirementNodeById(id) {
  const found = findRequirementNodeWithParent(eventRequirementTreeState, id);
  if (!found || !found.parent) return null;
  const index = found.parent.children.findIndex((child) => child.id === id);
  if (index < 0) return null;
  return found.parent.children.splice(index, 1)[0];
}

function isRequirementNodeDescendant(node, candidateId) {
  if (!node || !Array.isArray(node.children)) return false;
  return node.children.some((child) => child.id === candidateId || isRequirementNodeDescendant(child, candidateId));
}

function moveRequirementNode(nodeId, targetGroupId, targetIndex) {
  if (!eventRequirementTreeState || nodeId === eventRequirementTreeState.id) return;
  const targetLookup = findRequirementNodeWithParent(eventRequirementTreeState, targetGroupId);
  if (!targetLookup || targetLookup.node.nodeType !== "Group") return;

  const draggedLookup = findRequirementNodeWithParent(eventRequirementTreeState, nodeId);
  if (!draggedLookup || !draggedLookup.parent) return;
  if (nodeId === targetGroupId || isRequirementNodeDescendant(draggedLookup.node, targetGroupId)) return;

  const sourceChildren = draggedLookup.parent.children;
  const sourceIndex = sourceChildren.findIndex((child) => child.id === nodeId);
  if (sourceIndex < 0) return;
  const [movedNode] = sourceChildren.splice(sourceIndex, 1);

  const targetChildren = targetLookup.node.children;
  let insertionIndex = Math.max(0, Math.min(targetIndex, targetChildren.length));
  if (draggedLookup.parent.id === targetGroupId && sourceIndex < insertionIndex) {
    insertionIndex -= 1;
  }

  targetChildren.splice(insertionIndex, 0, movedNode);
}

function defaultEventOption() {
  return { text: "", effects: [] };
}

function bindEventOptionRow(row) {
  const removeButton = row.querySelector("[data-remove-event-option]");
  const addEffectButtonInRow = row.querySelector("[data-add-event-option-effect]");
  const effectsContainerInRow = row.querySelector("[data-event-option-effects-container]");

  if (removeButton) {
    removeButton.addEventListener("click", () => {
      if (eventOptionsContainer && eventOptionsContainer.querySelectorAll("[data-event-option-row]").length > 1) {
        row.remove();
        reindexEventOptions();
      }
    });
  }

  if (addEffectButtonInRow) {
    addEffectButtonInRow.addEventListener("click", () => {
      const optionIndex = Array.from(eventOptionsContainer.querySelectorAll("[data-event-option-row]")).indexOf(row);
      const effectIndex = effectsContainerInRow.querySelectorAll("[data-effect-row]").length;
      const html = eventOptionEffectTemplate.innerHTML
        .replaceAll("__OPTION_INDEX__", String(optionIndex))
        .replaceAll("__INDEX__", String(effectIndex));
      effectsContainerInRow.insertAdjacentHTML("beforeend", html);
      const effectRow = effectsContainerInRow.querySelectorAll("[data-effect-row]")[effectIndex];
      bindEventOptionEffectRow(effectRow, effectsContainerInRow);
      reindexEventOptions();
      applyLanguage(body.dataset.lang || "en");
    });
  }

  if (effectsContainerInRow) {
    effectsContainerInRow.querySelectorAll("[data-effect-row]").forEach((effectRow) => bindEventOptionEffectRow(effectRow, effectsContainerInRow));
  }
}

function bindEventOptionEffectRow(row, container) {
  const typeSelectInRow = row.querySelector("[data-effect-value-type]");
  if (typeSelectInRow) typeSelectInRow.addEventListener("change", () => syncEffectRow(row));
  const removeButton = row.querySelector("[data-remove-effect]");
  if (removeButton) {
    removeButton.addEventListener("click", () => {
      row.remove();
      reindexEventOptions();
    });
  }
  syncEffectRow(row);
}

function reindexEventOptions() {
  if (!eventOptionsContainer) return;
  eventOptionsContainer.querySelectorAll("[data-event-option-row]").forEach((row, optionIndex) => {
    row.querySelectorAll("[name]").forEach((field) => {
      field.name = field.name
        .replace(/AdvanceForm\.EventOptions\[\d+\]/, `AdvanceForm.EventOptions[${optionIndex}]`)
        .replace(/\.Effects\[\d+\]/, (() => {
          const effectRow = field.closest("[data-effect-row]");
          const container = effectRow ? effectRow.parentElement : null;
          const effectIndex = container ? Array.from(container.querySelectorAll("[data-effect-row]")).indexOf(effectRow) : 0;
          return `.Effects[${effectIndex}]`;
        })());
    });
    const effectsContainerInRow = row.querySelector("[data-event-option-effects-container]");
    if (effectsContainerInRow) {
      effectsContainerInRow.querySelectorAll("[data-effect-row]").forEach((effectRow) => {
        bindEventOptionEffectRow(effectRow, effectsContainerInRow);
      });
    }
  });
}

function addEventOptionRow(option = null) {
  if (!eventOptionTemplate || !eventOptionsContainer) return;
  const optionIndex = eventOptionsContainer.querySelectorAll("[data-event-option-row]").length;
  eventOptionsContainer.insertAdjacentHTML("beforeend", eventOptionTemplate.innerHTML.replaceAll("__OPTION_INDEX__", String(optionIndex)));
  const row = eventOptionsContainer.querySelectorAll("[data-event-option-row]")[optionIndex];
  bindEventOptionRow(row);
  const textField = row.querySelector(`[name="AdvanceForm.EventOptions[${optionIndex}].Text"]`);
  if (textField) textField.value = option?.text ?? "";

  const effectsContainerInRow = row.querySelector("[data-event-option-effects-container]");
  if (effectsContainerInRow) {
    effectsContainerInRow.innerHTML = "";
    const effects = option
      ? (Array.isArray(option.effects) ? option.effects : [])
      : [defaultEffect()];
    effects.forEach((effect, effectIndex) => {
      const html = eventOptionEffectTemplate.innerHTML
        .replaceAll("__OPTION_INDEX__", String(optionIndex))
        .replaceAll("__INDEX__", String(effectIndex));
      effectsContainerInRow.insertAdjacentHTML("beforeend", html);
      const effectRow = effectsContainerInRow.querySelectorAll("[data-effect-row]")[effectIndex];
      bindEventOptionEffectRow(effectRow, effectsContainerInRow);
      setEffectRowValues(effectRow, `EventOptions[${optionIndex}].Effects`, effectIndex, effect);
    });
  }

  applyLanguage(body.dataset.lang || "en");
  reindexEventOptions();
}

function buildEventRequirementRows(requirements) {
  buildEventRequirementTree(requirements);
}

function buildEventOptionRows(options) {
  if (!eventOptionsContainer) return;
  eventOptionsContainer.innerHTML = "";
  const items = options && options.length > 0 ? options : [defaultEventOption()];
  items.forEach((option) => addEventOptionRow(option));
  reindexEventOptions();
}

function syncEventPrerequisites(selectedIds, currentContentId) {
  document.querySelectorAll("[data-event-prerequisite-row]").forEach((row) => {
    const checkbox = row.querySelector('input[type="checkbox"]');
    const rowEventId = row.dataset.eventId;
    if (!checkbox) return;
    checkbox.checked = Array.isArray(selectedIds) && selectedIds.includes(rowEventId);
    checkbox.disabled = Boolean(currentContentId) && rowEventId === currentContentId;
    if (checkbox.disabled) checkbox.checked = false;
  });
}

function updateModalModeText() {
  const dictionary = translations[body.dataset.lang || "en"] || translations.en;
  const isEdit = contentForm && contentForm.dataset.mode === "edit";
  if (contentModalTitle) {
    const key = isEdit ? contentModalTitle.dataset.editTitleKey : contentModalTitle.dataset.addTitleKey;
    if (key && dictionary[key]) {
      contentModalTitle.dataset.i18n = key;
      contentModalTitle.textContent = dictionary[key];
    }
  }
  if (submitContentButton) {
    const key = isEdit ? submitContentButton.dataset.editLabelKey : submitContentButton.dataset.addLabelKey;
    if (key && dictionary[key]) {
      submitContentButton.dataset.i18n = key;
      submitContentButton.textContent = dictionary[key];
    }
  }
}

function resetFormForAddMode() {
  if (!contentForm) return;
  const countryIdField = contentForm.querySelector('[name="AdvanceForm.CountryId"]');
  const countryId = countryIdField ? countryIdField.value : "";
  contentForm.reset();
  contentForm.action = contentForm.dataset.addAction;
  contentForm.dataset.mode = "add";
  if (contentIdInput) contentIdInput.value = "";
  if (countryIdField) countryIdField.value = countryId;
  if (typeSelect) typeSelect.value = "Advance";
  buildEffectRows(effectsContainer, effectTemplate, "Effects", [defaultEffect()]);
  buildEffectRows(leftEffectsContainer, leftEffectTemplate, "LeftEffects", [defaultEffect()]);
  buildEffectRows(rightEffectsContainer, rightEffectTemplate, "RightEffects", [defaultEffect()]);
  buildConstructionCostRows([{ resourceName: "", amount: 0 }]);
  buildProductionMethodRows([{ name: "", inputs: [{ resourceName: "", amount: 0 }], outputs: [{ resourceName: "", amount: 0 }] }]);
  buildEventRequirementRows(null);
  buildEventOptionRows([defaultEventOption()]);
  syncEventPrerequisites([], null);
  updateModalModeText();
  syncContentTypePanel();
}

function applyLawSelection(content) {
  const category = content.lawCategory ?? "";
  const subcategory = content.lawSubcategory ?? "";
  const isBuiltInCategory = Object.prototype.hasOwnProperty.call(lawCatalog, category);
  if (lawCategorySelect) lawCategorySelect.value = isBuiltInCategory ? category : "Custom";
  setNamedFieldValue("AdvanceForm.LawCustomCategory", isBuiltInCategory ? "" : category);
  rebuildLawSubcategories();
  if (!isBuiltInCategory) {
    setNamedFieldValue("AdvanceForm.LawCustomSubcategory", subcategory);
    return;
  }
  const builtInSubcategories = lawCatalog[category] || [];
  if (builtInSubcategories.includes(subcategory)) {
    if (lawSubcategorySelect) lawSubcategorySelect.value = subcategory;
    setNamedFieldValue("AdvanceForm.LawCustomSubcategory", "");
  } else {
    if (lawSubcategorySelect) lawSubcategorySelect.value = "Custom";
    setNamedFieldValue("AdvanceForm.LawCustomSubcategory", subcategory);
  }
}

function applySelectedContentToForm(content) {
  if (!contentForm || !content) return;
  const countryIdField = contentForm.querySelector('[name="AdvanceForm.CountryId"]');
  const countryId = countryIdField ? countryIdField.value : "";
  contentForm.reset();
  contentForm.action = contentForm.dataset.editAction;
  contentForm.dataset.mode = "edit";
  if (contentIdInput) contentIdInput.value = content.id ?? "";
  if (countryIdField) countryIdField.value = countryId;
  if (typeSelect) typeSelect.value = content.type ?? "Advance";
  setNamedFieldValue("AdvanceForm.Name", content.name ?? "");
  setNamedFieldValue("AdvanceForm.IsMajorReform", content.isMajorReform ?? false);
  setNamedFieldValue("AdvanceForm.NobilityEstateName", content.nobilityEstateName ?? "");
  setNamedFieldValue("AdvanceForm.BurghersEstateName", content.burghersEstateName ?? "");
  setNamedFieldValue("AdvanceForm.ClergyEstateName", content.clergyEstateName ?? "");
  setNamedFieldValue("AdvanceForm.PeasantsEstateName", content.peasantsEstateName ?? "");
  setNamedFieldValue("AdvanceForm.FoodConsumptionPerThousand", content.foodConsumptionPerThousand ?? 0);
  setNamedFieldValue("AdvanceForm.AssimilationConversionSpeed", content.assimilationConversionSpeed ?? 0);
  setNamedFieldValue("AdvanceForm.EstateClass", content.estateClass ?? "Upper");
  setNamedFieldValue("AdvanceForm.CanPromote", content.canPromote ?? false);
  setNamedFieldValue("AdvanceForm.PromotionSpeed", content.promotionSpeed ?? 0);
  setNamedFieldValue("AdvanceForm.MigrationSpeed", content.migrationSpeed ?? 0);
  setNamedFieldValue("AdvanceForm.PrivilegeEstateTarget", content.privilegeEstateTarget ?? "Nobles");
  setNamedFieldValue("AdvanceForm.PrivilegeCustomEstateName", content.privilegeCustomEstateName ?? "");
  setNamedFieldValue("AdvanceForm.SatisfactionBonusPercent", content.satisfactionBonusPercent ?? 0);
  setNamedFieldValue("AdvanceForm.EstatePowerPercent", content.estatePowerPercent ?? 0);
  setNamedFieldValue("AdvanceForm.LawEstatePreferenceTarget", content.lawEstatePreferenceTarget ?? "Nobles");
  setNamedFieldValue("AdvanceForm.LawCustomEstateName", content.lawCustomEstateName ?? "");
  setNamedFieldValue("AdvanceForm.ValueLeftLabel", content.valueLeftLabel ?? "");
  setNamedFieldValue("AdvanceForm.ValueRightLabel", content.valueRightLabel ?? "");
  setNamedFieldValue("AdvanceForm.BuildingConstructionScope", content.buildingConstructionScope ?? "AllLocations");
  setNamedFieldValue("AdvanceForm.BuildingDucatCost", content.buildingDucatCost ?? 0);
  setNamedFieldValue("AdvanceForm.BuildingTimeMonths", content.buildingTimeMonths ?? 0);
  setNamedFieldValue("AdvanceForm.EventDescription", content.eventDescription ?? "");
  setNamedFieldValue("AdvanceForm.EventYearStart", content.eventYearStart ?? "");
  setNamedFieldValue("AdvanceForm.EventYearEnd", content.eventYearEnd ?? "");
  setNamedFieldValue("AdvanceForm.EventTriggerMode", content.eventTriggerMode ?? "MonthlyChance");
  setNamedFieldValue("AdvanceForm.EventMonthlyChance", content.eventMonthlyChance ?? 0);
  applyLawSelection(content);
  buildEffectRows(effectsContainer, effectTemplate, "Effects", content.effects);
  buildEffectRows(leftEffectsContainer, leftEffectTemplate, "LeftEffects", content.leftEffects);
  buildEffectRows(rightEffectsContainer, rightEffectTemplate, "RightEffects", content.rightEffects);
  buildConstructionCostRows(content.constructionCosts);
  buildProductionMethodRows(content.productionMethods);
  buildEventRequirementRows(content.eventRequirementTree ?? content.eventRequirements);
  buildEventOptionRows(content.eventOptions);
  syncEventPrerequisites(content.eventPrerequisiteIds ?? [], content.id ?? null);
  updateModalModeText();
  syncContentTypePanel();
}

function cssEscape(value) {
  if (window.CSS && typeof window.CSS.escape === "function") return window.CSS.escape(value);
  return value.replace(/([ #;?%&,.+*~':"!^$[\]()=>|/@])/g, "\\$1");
}

const savedLanguage = localStorage.getItem("planner-language") || "en";
const savedTheme = localStorage.getItem("planner-theme") || "dark";
if (contentForm) contentForm.dataset.mode = "add";

applyLanguage(savedLanguage);
applyTheme(savedTheme);
syncContentTypePanel();
buildEventRequirementRows(null);

languageButtons.forEach((button) => button.addEventListener("click", () => applyLanguage(button.dataset.language)));
if (themeToggle) themeToggle.addEventListener("click", () => applyTheme(body.classList.contains("theme-dark") ? "light" : "dark"));
if (addContentButton) addContentButton.addEventListener("click", resetFormForAddMode);
if (editContentButton) editContentButton.addEventListener("click", () => applySelectedContentToForm(selectedContentData));
if (typeSelect) typeSelect.addEventListener("change", syncContentTypePanel);
if (canPromoteToggle) canPromoteToggle.addEventListener("change", syncPromotionSpeed);
if (eventTriggerModeSelect) eventTriggerModeSelect.addEventListener("change", syncEventTriggerFields);
if (privilegeEstateSelect) privilegeEstateSelect.addEventListener("change", syncPrivilegeCustomEstate);
if (lawCategorySelect) lawCategorySelect.addEventListener("change", () => { rebuildLawSubcategories(); syncLawFields(); });
if (lawSubcategorySelect) lawSubcategorySelect.addEventListener("change", syncLawFields);
if (lawEstateSelect) lawEstateSelect.addEventListener("change", syncLawFields);

if (effectsContainer) effectsContainer.querySelectorAll("[data-effect-row]").forEach((row) => bindEffectRow(row, effectsContainer, "Effects"));
if (leftEffectsContainer) leftEffectsContainer.querySelectorAll("[data-effect-row]").forEach((row) => bindEffectRow(row, leftEffectsContainer, "LeftEffects"));
if (rightEffectsContainer) rightEffectsContainer.querySelectorAll("[data-effect-row]").forEach((row) => bindEffectRow(row, rightEffectsContainer, "RightEffects"));
if (constructionCostsContainer) constructionCostsContainer.querySelectorAll("[data-resource-row]").forEach((row) => bindResourceRow(row, constructionCostsContainer, reindexConstructionCosts));
if (productionMethodsContainer) productionMethodsContainer.querySelectorAll("[data-production-method-row]").forEach((row) => bindProductionMethodRow(row));
if (eventOptionsContainer) eventOptionsContainer.querySelectorAll("[data-event-option-row]").forEach((row) => bindEventOptionRow(row));

if (addEffectButton) addEffectButton.addEventListener("click", () => { addEffectRow(effectsContainer, effectTemplate, "Effects"); reindexEffectRows(effectsContainer, "Effects"); });
if (addLeftEffectButton) addLeftEffectButton.addEventListener("click", () => { addEffectRow(leftEffectsContainer, leftEffectTemplate, "LeftEffects"); reindexEffectRows(leftEffectsContainer, "LeftEffects"); });
if (addRightEffectButton) addRightEffectButton.addEventListener("click", () => { addEffectRow(rightEffectsContainer, rightEffectTemplate, "RightEffects"); reindexEffectRows(rightEffectsContainer, "RightEffects"); });
if (addConstructionCostButton) addConstructionCostButton.addEventListener("click", () => { addConstructionCostRow(); reindexConstructionCosts(); });
if (addProductionMethodButton) addProductionMethodButton.addEventListener("click", () => addProductionMethodRow());
if (addEventOptionButton) addEventOptionButton.addEventListener("click", () => addEventOptionRow(defaultEventOption()));

if (eventRequirementBuilder) {
  eventRequirementBuilder.addEventListener("click", (event) => {
    const target = event.target.closest("button");
    if (!target || !eventRequirementTreeState) return;

    const nodeId = target.dataset.reqNodeId;
    if (target.hasAttribute("data-req-add-condition")) {
      const group = findRequirementNodeWithParent(eventRequirementTreeState, nodeId)?.node;
      if (group && group.nodeType === "Group") {
        group.children.push(createRequirementCondition(""));
        renderRequirementTree();
      }
      return;
    }

    if (target.dataset.reqAddGroup) {
      const group = findRequirementNodeWithParent(eventRequirementTreeState, nodeId)?.node;
      if (group && group.nodeType === "Group") {
        group.children.push(createRequirementGroup(target.dataset.reqAddGroup, [createRequirementCondition("")]));
        renderRequirementTree();
      }
      return;
    }

    if (target.dataset.reqSetOperator) {
      const group = findRequirementNodeWithParent(eventRequirementTreeState, nodeId)?.node;
      if (group && group.nodeType === "Group") {
        group.groupOperator = target.dataset.reqSetOperator === "Or" ? "Or" : "And";
        renderRequirementTree();
      }
      return;
    }

    if (target.hasAttribute("data-req-remove-node")) {
      removeRequirementNodeById(nodeId);
      renderRequirementTree();
    }
  });

  eventRequirementBuilder.addEventListener("input", (event) => {
    const input = event.target.closest("[data-req-expression]");
    if (!input || !eventRequirementTreeState) return;
    const lookup = findRequirementNodeWithParent(eventRequirementTreeState, input.dataset.reqNodeId);
    if (!lookup || lookup.node.nodeType !== "Condition") return;
    lookup.node.expression = input.value;
    serializeRequirementTree();
  });

  eventRequirementBuilder.addEventListener("dragstart", (event) => {
    const node = event.target.closest(".requirement-node[draggable='true']");
    if (!node) return;
    draggedRequirementNodeId = node.dataset.reqNodeId;
    event.dataTransfer.effectAllowed = "move";
  });

  eventRequirementBuilder.addEventListener("dragend", () => {
    draggedRequirementNodeId = null;
    eventRequirementBuilder.querySelectorAll("[data-req-drop-zone]").forEach((zone) => zone.classList.remove("is-active"));
  });

  eventRequirementBuilder.addEventListener("dragover", (event) => {
    const zone = event.target.closest("[data-req-drop-zone]");
    if (!zone || !draggedRequirementNodeId) return;
    event.preventDefault();
    zone.classList.add("is-active");
    event.dataTransfer.dropEffect = "move";
  });

  eventRequirementBuilder.addEventListener("dragleave", (event) => {
    const zone = event.target.closest("[data-req-drop-zone]");
    if (zone) zone.classList.remove("is-active");
  });

  eventRequirementBuilder.addEventListener("drop", (event) => {
    const zone = event.target.closest("[data-req-drop-zone]");
    if (!zone || !draggedRequirementNodeId) return;
    event.preventDefault();
    zone.classList.remove("is-active");
    moveRequirementNode(
      draggedRequirementNodeId,
      zone.dataset.targetGroupId,
      Number.parseInt(zone.dataset.targetIndex || "0", 10)
    );
    draggedRequirementNodeId = null;
    renderRequirementTree();
  });
}
