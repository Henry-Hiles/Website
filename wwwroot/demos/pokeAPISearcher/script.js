const pokemonCount = 12
const url = new URL(window.location.href)
const urlPokedex = url.searchParams.get("pokedex")
const urlType = url.searchParams.get("type")
const cards = document.querySelector("#cards")
const more = document.querySelector("#more")
const template = document.querySelector("template")
const pokedexSelect = document.querySelector("#pokedex")
const typeSelect = document.querySelector("#type")
let pokedex = null
let type = null
let disabledLoading = false
let currentOffset = 0

const disableLoading = () => {
    disabledLoading = true
    more.classList.add("hidden")
}

const noPokemon = () => {
    const h2 = document.createElement("h2")
    h2.innerText = "No Pokemon"
    h2.id = "none"
    cards.append(h2)
    disableLoading()
}

const checkForMorePokemon = async () => {
    const more = document.querySelector("#more")
    const moreOffset = more.offsetTop + more.clientHeight
    const pageOffset = window.pageYOffset + window.innerHeight

    if (pageOffset > moreOffset - 20) addPokemon()
}

const addPokemon = async () => {
    if (disabledLoading) return
    const offset = currentOffset
    currentOffset += pokemonCount
    const pokemonUrls = []
    if (type && pokedex.name != "national") {
        pokedex.pokemon_entries.forEach((entry) => {
            const url = entry.pokemon_species.url.replace("-species", "")
            type.pokemon.forEach((typePokemon) => {
                if (typePokemon.pokemon.url == url) {
                    pokemonUrls.push(typePokemon)
                }
            })
        })
        if (!pokemonUrls.length) return noPokemon()
    }
    if ((type && !type.pokemon.length) || !pokedex.pokemon_entries.length)
        return noPokemon()
    const addCard = async (i) => {
        try {
            const pokemonResponse = await fetch(
                type
                    ? (pokemonUrls.length ? pokemonUrls : type.pokemon)[
                          i + offset
                      ].pokemon.url
                    : pokedex.pokemon_entries[
                          i + offset
                      ].pokemon_species.url.replace("-species", "")
            )
            pokemon = await pokemonResponse.json()
            const card = cards.children[offset + i]
            const img = card.querySelector("[data-image]")
            const types = card.querySelector("[data-types]")
            img.addEventListener("load", () => img.classList.remove("skeleton"))
            img.src = pokemon.sprites.front_default
            card.querySelector("[data-name]").textContent = pokemon.name

            if (pokemon.types.length == 1) {
                types.children[1].remove()
            }

            pokemon.types.forEach((type, i) => {
                const typeElement = types.children[i]
                typeElement.addEventListener("click", () => {
                    typeSelect.value = type.type.url.substring(
                        31,
                        type.type.url.length - 1
                    )
                    changeType()
                })
                const typeName = type.type.name
                const typeImage = typeElement.children[0]
                typeImage.addEventListener("load", () =>
                    typeImage.classList.remove("skeleton")
                )
                typeImage.src = `images/${typeName}.png`
                const typeNameElement = typeElement.children[1]
                typeNameElement.innerText = typeName
                typeNameElement.classList.remove("skeleton", "skeleton-text")
            })
        } catch (error) {
            console.log(error)
        }
    }
    ;[...Array(pokemonCount)].forEach((_, i) => {
        if (!disabledLoading) {
            if (
                i + offset ==
                (pokemonUrls.length ||
                    (type
                        ? type.pokemon.length
                        : pokedex.pokemon_entries.length))
            )
                disableLoading()
            else {
                cards.append(template.content.cloneNode(true))
                addCard(i).catch((error) => console.log(error))
            }
        }
    })
}
;(async () => {
    const defaultPokedex = await fetch(
        `https://pokeapi.co/api/v2/pokedex/${urlPokedex || 1}/`
    )
    pokedex = await defaultPokedex.json()
    const pokedexResponse = await fetch(
        `https://pokeapi.co/api/v2/pokedex?limit=9999`
    )
    const pokedexResult = await pokedexResponse.json()
    pokedexResult.results.forEach((pokedex) => {
        const option = document.createElement("option")
        option.innerText = pokedex.name
        option.value = pokedex.url.substring(34, pokedex.url.length - 1)
        pokedexSelect.append(option)
    })
    if (urlPokedex) pokedexSelect.value = urlPokedex

    const typeResponse = await fetch(`https://pokeapi.co/api/v2/type`)
    const typeResult = await typeResponse.json()
    typeResult.results.sort((previous, current) =>
        previous.name > current.name ? 1 : current.name > previous.name ? -1 : 0
    )
    typeResult.results.forEach((type) => {
        const option = document.createElement("option")
        option.innerText = type.name
        option.value = type.url.substring(31, type.url.length - 1)
        typeSelect.append(option)
    })
    if (urlType) {
        typeSelect.value = urlType
        changeType()
    }
    addPokemon()

    more.addEventListener("click", addPokemon)
    document.addEventListener("scroll", checkForMorePokemon)
})()

const changeType = async () => {
    if (typeSelect.value != "all") {
        url.searchParams.set("type", typeSelect.value)
        const responce = await fetch(
            `https://pokeapi.co/api/v2/type/${typeSelect.value}/`
        )
        type = await responce.json()
    } else {
        url.searchParams.delete("type")
        type = null
    }
    cards.innerHTML = ""
    currentOffset = 0
    disabledLoading = false
    url.search = url.searchParams.toString()
    history.replaceState(null, "", url)
    addPokemon()
}

typeSelect.addEventListener("change", async () => await changeType())

pokedexSelect.addEventListener("change", async () => {
    pokedexSelect.value == 1
        ? url.searchParams.delete("pokedex")
        : url.searchParams.set("pokedex", pokedexSelect.value)
    const responce = await fetch(
        `https://pokeapi.co/api/v2/pokedex/${pokedexSelect.value}/`
    )
    pokedex = await responce.json()

    cards.innerHTML = ""
    currentOffset = 0
    disabledLoading = false
    url.search = url.searchParams.toString()
    history.replaceState(null, "", url)
    addPokemon()
})
