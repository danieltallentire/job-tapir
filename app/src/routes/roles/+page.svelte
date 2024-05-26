<script lang="ts">

import {onMount} from "svelte";
import Grid from "gridjs-svelte"
    import { get_slot_changes } from "svelte/internal";
    import { html } from "gridjs";

let tags = [];
let roles = [];
let columns = [
    {name: "Company", data: (row) => row.company.name}, 
    {name: "Job Title", data: (row) => row.title },
    {name: "Salary", data: (row) => row.hasAdvertisedSalary ? row.advertisedSalaryFloor + " - " + row.advertisedSalaryCeiling : "Unknown" },
    {name: "Remote?", data: (row) => row.remoteWorking.workType },
    {name: "Tags", data: (row) => row.tags.map(t => tags[t]), formatter: (_, row) => html(renderTags(row.cells[4].data)) } 

];

function renderTags(tags) {
    return `
        <ul class="tags">
            ${tags.map(tag => `<li class="tag">${tag}</li>`).join('')}
        </ul>
    `;
}

onMount( async () => {
    const res = await fetch("http://localhost:8000/roles");
    const content = await res.json();
    getTags();
    roles = content;
});

async function getTags()
{
    const res = await fetch("http://localhost:8000/tags");
    const content = await res.json();

    content.forEach(t => {
        tags[t.id] = t.name;
    });
}

function tagClick(event) {
    event.element.data["id"];
}

function tagKeyDown(event) {

}

</script>


<h2>Roles</h2>

<ul class="tags" id="tag-selector">
    {#each tags as tag, id}
        <li class="tag" data-id={id} on:click={tagClick} on:keydown={tagKeyDown}>{tag}</li>
    {/each}
</ul>

<Grid data={roles} columns={columns} sort=true search=true/>


<style>
	@import "https://cdn.jsdelivr.net/npm/gridjs/dist/theme/mermaid.min.css";

    :global(ul.tags) {
    list-style-type: none;
    padding: 0;
    margin: 0;
    display: flex;
    flex-wrap: wrap;
  }

  :global(li.tag) {
    background-color: #2aa826;
    border: 1px solid #ccc;
    border-radius: 12px;
    padding: 5px 10px;
    margin: 5px;
    display: inline-block;
  }

</style>