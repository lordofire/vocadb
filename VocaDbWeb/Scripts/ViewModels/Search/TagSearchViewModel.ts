﻿
module vdb.viewModels.search {

	import dc = vdb.dataContracts;

	export class TagSearchViewModel extends SearchCategoryBaseViewModel<dc.TagApiContract> {

		constructor(searchViewModel: SearchViewModel, private tagRepo: rep.TagRepository) {

			super(searchViewModel);

			this.allowAliases.subscribe(this.updateResultsWithTotalCount);
			this.categoryName.subscribe(this.updateResultsWithTotalCount);

			this.loadResults = (pagingProperties, searchTerm, tag, status, callback) => {

				this.tagRepo.getList(pagingProperties, searchTerm, this.allowAliases(), this.categoryName(), callback);

			}

		}

		public allowAliases = ko.observable(false);
		public categoryName = ko.observable("");

	}

}